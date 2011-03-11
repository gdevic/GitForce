using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Git4Win
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
        public ClassRemotes Remotes = new ClassRemotes();

        /// <summary>
        /// Set of active commits currently existing for this repo
        /// </summary>
        public ClassCommits Commits = new ClassCommits();

        /// <summary>
        /// Set of branches for the current repo
        /// </summary>
        public ClassBranches Branches = new ClassBranches();

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
        /// Constructor that sets the root of a new repository
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
            UserName = ClassConfig.GetLocal(this, "user.name");
            if (ClassUtils.IsLastError())
                return false;
            UserEmail = ClassConfig.GetLocal(this, "user.email");
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
        /// Transforms a Windows absolute path to the repository relative path with
        /// slashes changes. This path format is suitable to send to git commands.
        /// </summary>
        public string Win2GitPath(string path)
        {
            string s = path.Substring(Root.Length + 1)
                           .Replace('\\', '/');
            return s;
        }

        /// <summary>
        /// Add untracked files to Git repository
        /// </summary>
        public void GitAdd(List<string> files)
        {
            string list = string.Join(" ", files.ToArray());
            App.PrintStatusMessage("Adding " + list);
            Run("add -- " + list);
        }

        /// <summary>
        /// Update modified files
        /// </summary>
        public void GitUpdate(List<string> files)
        {
            string list = string.Join(" ", files.ToArray());
            App.PrintStatusMessage("Updating " + list);
            Run("add -- " + list);
        }

        /// <summary>
        /// Delete a list of files
        /// </summary>
        public void GitDelete(List<string> files)
        {
            string list = string.Join(" ", files.ToArray());
            App.PrintStatusMessage("Removing " + list);
            Run("rm -- " + list);            
        }

        /// <summary>
        /// Rename a list of files (huh?)
        /// </summary>
        public void GitRename(List<string> files)
        {
            string list = string.Join(" ", files.ToArray());
            App.PrintStatusMessage("Renaming " + list);
            Run("add -- " + list);
        }

        /// <summary>
        /// Revert a list of files
        /// </summary>
        public void GitRevert(List<string> files)
        {
            string list = string.Join(" ", files.ToArray());
            App.PrintStatusMessage("Reverting " + list);
            Run("checkout -- " + list);
        }

        ///// <summary>
        ///// Repo class function that runs a git command in the context of a repository.
        ///// </summary>
        //public string Run(string args)
        //{
        //    // The Windows limit to the command line argument length is about 8K
        //    // We may hit that limit when doing operations on a large number of files
        //    if (args.Length < 1000)
        //        return Run(args, null);

        //    // Partition the args into "[command] -- [set of file chunks < 8000 chars]"
        //    // Basically we have to rebuild the command into multiple instances with
        //    // same command but with file lists not larger than about 8K
        //    int i = args.IndexOf(" -- ") + 3;
        //    string cmd = args.Substring(0, i + 1);
        //    args = args.Substring(i);

        //    // Add files individually up to the length limit
        //    string[] files = args.Split(' ');
        //    i = 0;
        //    do
        //    {
        //        StringBuilder batch = new StringBuilder(8100);
        //        while (batch.Length < 8000 && i < files.Length)
        //            batch.Append(files[i++] + " ");

        //        Run(cmd + batch, null);

        //    } while (i < files.Length);

        //    return "";
        //}

        /// <summary>
        /// Repo class function that runs a git command in the context of a repository.
        /// </summary>
        public string Run(string args)
        {
            App.Log.Print("git " + args);
            string output = "";
            ClassUtils.ClearLastError();

            try
            {
                Directory.SetCurrentDirectory(Root);

                // Set the HTTPS password
                string password = Remotes.GetPassword("");
                ClassExecute.AddEnvar("PASSWORD", password);

                output = ClassGit.Run(args);
            }
            catch (Exception ex)
            {
                ClassUtils.LastError = ex.Message;
            }

            return output;            
        }
    }
}
