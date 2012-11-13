using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Class containing a single repository and a set of functions operating on it
    /// </summary>
    [Serializable]
    public class ClassRepo
    {
        /// <summary>
        /// Root local directory of the repository
        /// </summary>
        public string Root;

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
        /// Current format can be tree view or list view
        /// </summary>
        public bool IsTreeView = true;

        /// <summary>
        /// Current sorting rule for the view
        /// </summary>
        public GitDirectoryInfo.SortBy SortBy = GitDirectoryInfo.SortBy.Name;

        /// <summary>
        /// Stores a set of paths that are expanded in the left pane view.
        /// Although used only by the view, keeping it here saves it across
        /// the sessions.
        /// WAR: This is marked as Non-Serialized since Mono 2.6.7 does not support serialization of HashSet.
        /// </summary>
        [NonSerialized]
        private HashSet<string> _viewExpansionSet = new HashSet<string>();

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
        /// Mark a specific path as expanded in the view
        /// </summary>
        public void ExpansionSet(string path)
        {
            _viewExpansionSet.Add(path);
        }

        /// <summary>
        /// Mark a specific path as collapsed in the view,
        /// or remove all paths from the list of expanded paths (if the path given is null)
        /// </summary>
        public void ExpansionReset(string path)
        {
            if (path == null)
                _viewExpansionSet = new HashSet<string>();
            else
                _viewExpansionSet.Remove(path);
        }

        /// <summary>
        /// Return true if the path is marked as expanded
        /// </summary>
        public bool IsExpanded(string path)
        {
            return _viewExpansionSet.Contains(path);
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
        /// Add untracked files to Git repository
        /// </summary>
        public void GitAdd(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Adding " + list);
            RunCmd("add -- " + list);
        }

        /// <summary>
        /// Update modified files
        /// </summary>
        public void GitUpdate(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Updating " + list);
            RunCmd("add -- " + list);
        }

        /// <summary>
        /// Delete a list of files
        /// </summary>
        public void GitDelete(List<string> files)
        {
            GitDelete("", files);
        }

        public void GitDelete(string tag, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Removing " + list);
            RunCmd("rm " + tag + " -- " + list);
        }

        /// <summary>
        /// Rename a list of files
        /// </summary>
        public void GitRename(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Renaming " + list);
            RunCmd("add -- " + list);
        }

        /// <summary>
        /// Moves a file to a different name or different location
        /// </summary>
        public void GitMove(string srcFile, string dstFile)
        {
            App.PrintStatusMessage(string.Format("Moving {0} to {1}", srcFile, dstFile));
            RunCmd("mv \"" + srcFile + "\" \"" + dstFile + "\"");
        }

        /// <summary>
        /// Revert a list of files
        /// </summary>
        public void GitRevert(List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Reverting " + list);
            RunCmd("checkout -- " + list);
        }

        /// <summary>
        /// Reset a list of files to a specific head.
        /// Returns true if a git command succeeded, fail otherwise.
        /// </summary>
        public bool GitReset(string head, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage(string.Format("Resetting to {0}: {1}", head, list));
            return RunCmd("reset " + head + " -- " + list).Success();
        }

        public void GitDiff(string tag, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Diffing " + list);
            RunCmd("difftool " + ClassDiff.GetDiffCmd() + " " + tag + " -- " + list, true);
        }

        /// <summary>
        /// Commit a list of files
        /// </summary>
        public void GitCommit(string cmd, bool isAmend, List<string> files)
        {
            string list = QuoteAndFlattenPaths(files);
            App.PrintStatusMessage("Submit " + list);

            // See below Run() for the description of the problem with long commands.
            // The Run() function breaks any command into chunks of 2000 characters or less
            // and issues them separately. This can be done on every command except 'commit'
            // since that would introduce multiple commits, which is probably not what the user
            // wants. Hence, we break commit at this level into an initial commit of a single
            // file (the first file on the list), and append for each successive chunk.
            if (isAmend == false && list.Length >= 2000)
            {
                RunCmd("commit " + cmd + " -- " + "\"" + files[0] + "\"");
                isAmend = true;
                files.RemoveAt(0);
                list = QuoteAndFlattenPaths(files);
            }
            RunCmd(string.Format("commit {0} {1} -- {2}", cmd, isAmend ? "--amend" : "", list));
        }

        /// <summary>
        /// Repo class function that runs a git command in the context of a repository.
        /// Use this function with all user-initiated commands in order to have them printed into the status window.
        /// </summary>
        public ExecResult RunCmd(string args, bool async = false)
        {
            // Print the actual command line to the status window only if user selected that setting
            if (Properties.Settings.Default.logCommands)
                App.PrintStatusMessage("git " + args);

            // Run the command and print the response to the status window in any case
            ExecResult result = Run(args, async);
            if (result.stdout.Length > 0)
                App.PrintStatusMessage(result.stdout);

            // If the command caused an error, print it also
            if (result.Success() == false)
                App.PrintStatusMessage(result.stderr);

            return result;
        }

        /// <summary>
        /// Repo class function that runs a git command in the context of a repository.
        /// </summary>
        public ExecResult Run(string args, bool async = false)
        {
            ExecResult output = new ExecResult();
            try
            {
                Directory.SetCurrentDirectory(Root);

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
                args = args.Substring(i);

                // Add files individually up to the length limit
                string[] files = args.Split(' ');
                i = 0;
                do
                {
                    StringBuilder batch = new StringBuilder(2100);
                    while (batch.Length < 2000 && i < files.Length)
                        batch.Append(files[i++] + " ");

                    output = ClassGit.Run(cmd + batch, async);
                    if (output.Success() == false)
                        break;
                } while (i < files.Length);
            }
            catch (Exception ex)
            {
                App.PrintLogMessage(ex.Message);
            }

            return output;
        }
    }
}