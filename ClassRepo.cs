using System;
using System.Collections;
using System.Collections.Generic;

namespace git4win
{
    /// <summary>
    /// Class containing a single repository
    /// </summary>
    [Serializable]
    public class ClassRepo
    {
        /// <summary>
        /// Root local directory of the repository
        /// </summary>
        public string root;

        /// <summary>
        /// Set of remotes associated with this repository
        /// </summary>
        public ClassRemotes remotes = new ClassRemotes();

        /// <summary>
        /// Set of active commits currently existing for this repo
        /// </summary>
        public ClassCommits commits = new ClassCommits();

        /// <summary>
        /// Stores the set of paths that are expanded in the left pane view.
        /// Although used only by the view, keeping it here saves it across
        /// the sessions.
        /// </summary>
        private HashSet<string> viewExpansionSet = new HashSet<string>();

        /// <summary>
        /// Constructor that sets the local root
        /// </summary>
        /// <param name="newRoot">Root of the repository</param>
        public ClassRepo(string newRoot)
        {
            root = newRoot;
        }

        /// <summary>
        /// Set a path as expanded
        /// </summary>
        public void ExpansionSet(string path)
        {
            viewExpansionSet.Add(path);
        }

        /// <summary>
        /// Remove path from the expanded list or clean the complete list if
        /// the path argument is null
        /// </summary>
        public void ExpansionReset(string path)
        {
            if (path == null)
                viewExpansionSet.Clear();
            else
                viewExpansionSet.Remove(path);
        }

        /// <summary>
        /// Return true if the path is marked as expanded
        /// </summary>
        /// <returns></returns>
        public bool isExpanded(string path)
        {
            return viewExpansionSet.Contains(path);
        }

        /// <summary>
        /// Transforms a Windows absolute path to the repository relative path with
        /// slashes changes. This path format is suitable to send to git commands.
        /// </summary>
        public string Win2GitPath(string path)
        {
            string s = path.Substring(root.Length + 1)
                           .Replace('\\', '/');
            return s;
        }

        /// <summary>
        /// Repo class function that runs a git command in the context of a repository
        /// which may not be the current repository (that global Git.Run() uses by default)
        /// </summary>
        /// <param name="cmd">Git command to run</param>
        /// <returns>Result text string from the git execution</returns>
        public string Run(string cmd, string path=null)
        {
            return App.Git.Run(cmd, this, path);
        }
    }
}
