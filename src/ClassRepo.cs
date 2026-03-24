using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Class containing a single repository and a set of functions operating on it
    /// </summary>
    [Serializable]
    public class ClassRepo : IComparable
    {
        /// <summary>
        /// Root local directory of the repository
        /// </summary>
        private string Root;

        /// <summary>
        /// User.name configuration setting for this repo
        /// </summary>
        public string UserName;

        /// <summary>
        /// User.email coniguration setting for this repo
        /// </summary>
        public string UserEmail;

        /// <summary>
        /// Set of remotes associated with this repository
        /// </summary>
        public readonly ClassRemotes Remotes = new ClassRemotes();

        /// <summary>
        /// Set of active commits currently existing for this repo
        /// </summary>
        public readonly ClassCommits Commits = new ClassCommits();

        /// <summary>
        /// Set of branches for the current repo
        /// </summary>
        public readonly ClassBranches Branches = new ClassBranches();

        /// <summary>
        /// Set of submodules for the current repo. This is refreshed
        /// on every App global refresh, so it does not need to be preserved across sessions.
        /// </summary>
        [NonSerialized]
        public ClassSubmodules Submodules;

        /// <summary>
        /// Current format can be tree view or list view
        /// </summary>
        public bool IsTreeView = true;

        /// <summary>
        /// Current sorting rule for the view
        /// </summary>
        public GitDirectoryInfo.SortBy SortBy = GitDirectoryInfo.SortBy.Name;

        /// <summary>
        /// Stores a set of paths that are expanded in the left pane view.
        /// Although used only by the view, keeping it here saves it across the sessions.
        /// WAR: This is marked as Non-Serialized since Mono 2.6.7 does not support serialization of HashSet.
        /// </summary>
        [NonSerialized]
        private HashSet<string> viewExpansionSet = new HashSet<string>();

        /// <summary>
        /// Stats of all files known to git in this repo. This status is refreshed
        /// on every App global refresh, so it does not need to be preserved across sessions.
        /// </summary>
        [NonSerialized]
        public ClassStatus Status;

        /// <summary>
        /// Class constructor
        /// </summary>
        public ClassRepo(string newRoot)
        {
            Root = newRoot;
        }

        /// <summary>
        /// Returns or sets the directory path of the repo
        /// </summary>
        /// <returns>Path to the git repository</returns>
        public string Path
        {
            get { return ClassUtils.GetCleanPath(Root); }
            set { Root = ClassUtils.GetCleanPath(value); }
        }

        /// <summary>
        /// Initializes repo class with user.name and user.email fields.
        /// Returns True if the repo appears valid and these settings are read.
        /// </summary>
        public bool Init()
        {
            // We assume that a valid git repo will always have core.bare entry
            if (ClassConfig.GetLocal(this, "core.bare") == string.Empty)
                return false;
            UserName = ClassConfig.GetLocal(this, "user.name");
            UserEmail = ClassConfig.GetLocal(this, "user.email");
            Status = new ClassStatus(this);
            return true;
        }

        /// <summary>
        /// ToString override returns the repository root path.
        /// </summary>
        public override string ToString()
        {
            return Root;
        }

        /// <summary>
        /// Implement default comparator so these classes can be sorted by their (root) name
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            ClassRepo a = obj as ClassRepo;
            return a != null ? ToString().CompareTo(a.ToString()) : 1;
        }

        /// <summary>
        /// Mark a specific path as expanded in the view
        /// </summary>
        public void ExpansionSet(string path)
        {
            viewExpansionSet.Add(path);
        }

        /// <summary>
        /// Mark a specific path as collapsed in the view,
        /// or remove all paths from the list of expanded paths (if the path given is null)
        /// </summary>
        public void ExpansionReset(string path)
        {
            if (path == null)
                viewExpansionSet = new HashSet<string>();
            else
                viewExpansionSet.Remove(path);
        }

        /// <summary>
        /// Return true if the path is marked as expanded
        /// </summary>
        public bool IsExpanded(string path)
        {
            return viewExpansionSet.Contains(path);
        }

        /// <summary>
        /// Converts a list of (relative) files into a quoted list,
        /// further flattened into a string suitable to send to a git command.
        /// </summary>
        private string QuoteAndFlattenPaths(List<string> files)
        {
            List<string> quoted = files.Select(file => "\"" + file + "\"").ToList();
            return string.Join(" ", quoted.ToArray());
        }

        /// <summary>
        /// Partition files into main repo files and submodule files, then run the given
        /// git command on each group in the appropriate directory context.
        /// </summary>
        /// <param name="cmdPrefix">Git command prefix (e.g., "add --")</param>
        /// <param name="files">List of file paths relative to repo root</param>
        private void RunCmdWithSubmoduleSupport(string cmdPrefix, List<string> files)
        {
            // Partition files: main repo vs submodules
            List<string> mainRepoFiles = new List<string>();
            Dictionary<string, List<string>> submoduleFiles = new Dictionary<string, List<string>>();

            if (Submodules != null)
            {
                foreach (string file in files)
                {
                    ClassSubmodules.Submodule sm;
                    string relativePath;
                    if (Submodules.GetContainingSubmodule(file, out sm, out relativePath))
                    {
                        if (!submoduleFiles.ContainsKey(sm.Path))
                            submoduleFiles[sm.Path] = new List<string>();
                        submoduleFiles[sm.Path].Add(relativePath);
                    }
                    else
                        mainRepoFiles.Add(file);
                }
            }
            else
                mainRepoFiles = files;

            // Run command for main repo files
            if (mainRepoFiles.Count > 0)
            {
                string list = QuoteAndFlattenPaths(mainRepoFiles);
                RunCmd(cmdPrefix + " " + list);
            }

            // Run command for each submodule
            foreach (var kvp in submoduleFiles)
            {
                string smPath = kvp.Key;
                string list = QuoteAndFlattenPaths(kvp.Value);
                RunCmd(cmdPrefix + " " + list, false, smPath);
            }
        }

        /// <summary>
        /// Add untracked files to Git repository
        /// </summary>
        public void GitAdd(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Adding " + list, MessageType.General);
            RunCmdWithSubmoduleSupport("add --", files);
            // Any git command that adds/updates files in the index might cause file check for TABs
            ClassTabCheck.CheckForTabs(files);
        }

        /// <summary>
        /// Update modified files
        /// </summary>
        public void GitUpdate(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Updating " + list, MessageType.General);
            RunCmdWithSubmoduleSupport("add --", files);
            // Any git command that adds/updates files in the index might cause file check for TABs
            ClassTabCheck.CheckForTabs(files);
        }

        /// <summary>
        /// Delete a list of files
        /// </summary>
        public void GitDelete(List<string> files) { GitDelete("", files); }
        public void GitDelete(string tag, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Removing " + list, MessageType.General);
            string cmd = string.IsNullOrEmpty(tag) ? "rm --" : "rm " + tag + " --";
            RunCmdWithSubmoduleSupport(cmd, files);
        }

        /// <summary>
        /// Rename a list of files
        /// </summary>
        public void GitRename(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Renaming " + list, MessageType.General);
            RunCmdWithSubmoduleSupport("add --", files);
            // Any git command that adds/updates files in the index might cause file check for TABs
            ClassTabCheck.CheckForTabs(files);
        }

        /// <summary>
        /// Moves a file to a different name or different location
        /// </summary>
        public void GitMove(string srcFile, string dstFile)
        {
            App.PrintStatusMessage(string.Format("Moving {0} to {1}", srcFile, dstFile), MessageType.General);
            RunCmd("mv \"" + srcFile + "\" \"" + dstFile + "\"");
        }

        /// <summary>
        /// Checkout a list of files
        /// </summary>
        public void GitCheckout(string options, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Checkout " + options + " " + list, MessageType.General);
            string cmd = string.IsNullOrEmpty(options) ? "checkout --" : "checkout " + options + " --";
            RunCmdWithSubmoduleSupport(cmd, files);
        }

        /// <summary>
        /// Revert a list of files
        /// </summary>
        public void GitRevert(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Reverting " + list, MessageType.General);
            RunCmdWithSubmoduleSupport("checkout --", files);
        }

        /// <summary>
        /// Reset a list of files to a specific head.
        /// Returns true if a git command succeeded, false otherwise.
        /// </summary>
        public bool GitReset(string head, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage(string.Format("Resetting to {0}: {1}", head, list), MessageType.General);
            RunCmdWithSubmoduleSupport("reset " + head + " --", files);
            return true; // Note: With submodule support, individual results are harder to track
        }

        /// <summary>
        /// Run the external diff command on a given file or a list of files
        /// </summary>
        public void GitDiff(string tag, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            if (list == "\"\"") // For now, we don't want to match all paths but only diff selected files
            {
                App.PrintStatusMessage("Diffing: No files selected and we don't want to match all paths.", MessageType.General);
                return;
            }
            App.PrintStatusMessage("Diffing " + list, MessageType.General);

            // Check if files are in a submodule (handle first file for simplicity)
            if (Submodules != null && files.Count > 0)
            {
                ClassSubmodules.Submodule sm;
                string relativePath;
                if (Submodules.GetContainingSubmodule(files[0], out sm, out relativePath))
                {
                    // Run diff in submodule context
                    List<string> smFiles = new List<string>();
                    foreach (string file in files)
                    {
                        string relPath;
                        ClassSubmodules.Submodule dummy;
                        if (Submodules.GetContainingSubmodule(file, out dummy, out relPath))
                            smFiles.Add(relPath);
                    }
                    string smList = QuoteAndFlattenPaths(smFiles);
                    Directory.SetCurrentDirectory(sm.Path);
                    ClassGit.Run("difftool " + ClassDiff.GetDiffCmd() + " " + tag + " -- " + smList, true);
                    return;
                }
            }

            RunCmd("difftool " + ClassDiff.GetDiffCmd() + " " + tag + " -- " + list, true);
        }

        /// <summary>
        /// Commit a list of files.
        /// Returns true if commit succeeded, false otherwise.
        /// </summary>
        public bool GitCommit(string cmd, bool isAmend, List<string> files)
        {
            ExecResult result;
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Submit " + list, MessageType.General);

            // See below Run() for the description of the problem with long commands.
            // The Run() function breaks any command into chunks of 2000 characters or less
            // and issues them separately. This can be done on every command except 'commit'
            // since that would introduce multiple commits, which is probably not what the user
            // wants. Hence, we break commit at this level into an initial commit of a single
            // file (the first file on the list), and append for each successive chunk.
            if (isAmend == false && list.Length >= 2000)
            {
                result = RunCmd("commit " + cmd + " -- " + "\"" + files[0] + "\"");
                if (result.Success() == false)
                    return false;
                isAmend = true;
                files.RemoveAt(0);
                list = QuoteAndFlattenPaths(files);
            }
            result = RunCmd(string.Format("commit {0} {1} -- {2}", cmd, isAmend ? "--amend" : "", list));
            return result.Success();
        }

        /// <summary>
        /// Run a git command with logging to status window.
        /// Use this function with all user-initiated commands.
        /// </summary>
        /// <param name="args">Git command arguments</param>
        /// <param name="async">If true, run asynchronously</param>
        /// <param name="workDir">Working directory (null = repo root)</param>
        /// <returns>ExecResult with stdout, stderr, and return code</returns>
        public ExecResult RunCmd(string args, bool async = false, string workDir = null)
        {
            // Print the actual command line to the status window only if user selected that setting
            if (Properties.Settings.Default.logCommands)
            {
                string msg = "git " + args;
                if (workDir != null && workDir != Root)
                    msg += " (in " + workDir + ")";
                App.PrintStatusMessage(msg, MessageType.Command);
            }

            // Run the command and print the response to the status window
            ExecResult result = Run(args, async, workDir);
            if (result.stdout.Length > 0)
                App.PrintStatusMessage(result.stdout, MessageType.Output);

            // If the command caused an error, print it also
            if (result.Success() == false)
                App.PrintStatusMessage(result.stderr, MessageType.Error);

            return result;
        }

        /// <summary>
        /// Run a git command in the context of a repository.
        /// </summary>
        /// <param name="args">Git command arguments</param>
        /// <param name="async">If true, run asynchronously</param>
        /// <param name="workDir">Working directory (null = repo root)</param>
        /// <returns>ExecResult with stdout, stderr, and return code</returns>
        public ExecResult Run(string args, bool async = false, string workDir = null)
        {
            ExecResult output = new ExecResult();
            try
            {
                Directory.SetCurrentDirectory(workDir ?? Root);

                // Set the HTTPS password
                string password = Remotes.GetPassword("");
                ClassUtils.AddEnvar("PASSWORD", password);

                // The Windows limit to the command line argument length is about 8K
                // We may hit that limit when doing operations on a large number of files.
                //
                // However, when sending a long list of files, git was hanging unless
                // the total length was much less than that, so I set it to about 2000 chars
                // which seemed to work fine.

                if (args.Length < 2000)
                    return ClassGit.Run(args, async);

                // Partition the args into "[command] -- [set of file chunks < 2000 chars]"
                // Basically we have to rebuild the command into multiple instances with
                // same command but with file lists not larger than about 2K
                int i = args.IndexOf(" -- ") + 3;
                string cmd = args.Substring(0, i + 1);
                args = args.Substring(i);       // We separate git command up to and until the list of files

                App.PrintLogMessage("Processing large amount of files: please wait...", MessageType.General);

                // Add files individually up to the length limit using the starting " file delimiter
                string[] files = args.Split(new [] {" \""}, StringSplitOptions.RemoveEmptyEntries);
                // Note: files in the list are now stripped from their initial " character!
                i = 0;
                do
                {
                    StringBuilder batch = new StringBuilder(2100);
                    while (batch.Length < 2000 && i < files.Length)
                        batch.Append("\"" + files[i++] + " ");

                    output = ClassGit.Run(cmd + batch, async);
                    if (output.Success() == false)
                        break;
                } while (i < files.Length);
            }
            catch (Exception ex)
            {
                App.PrintLogMessage(ex.Message, MessageType.Error);
            }
            return output;
        }
    }
}
