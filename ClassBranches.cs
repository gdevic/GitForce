using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// List of branches for the current repo, and a set of functions to operate on them
    /// </summary>
    public class ClassBranches
    {
        /// <summary>
        /// Name of the current branch
        /// </summary>
        public string current = null;

        /// <summary>
        /// List of local branches by their name
        /// </summary>
        public List<string> local = new List<string>();

        /// <summary>
        /// List of remote branches by their name
        /// </summary>
        public List<string> remote = new List<string>();

        /// <summary>
        /// Refresh the list of branches and assign local and remote
        /// </summary>
        public void Refresh()
        {
            local.Clear();
            remote.Clear();
            current = null;

            if (App.Repos.current != null)
            {
                string[] response = App.Git.Run("branch -a").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in response)
                {
                    // Recognize current branch - it is marked by an asterisk
                    if (s[0] == '*')
                        current = s.Replace("*", " ").Trim();

                    // Detect if a branch is local or remote and add it to the appropriate list
                    if (s.Contains(" remotes/"))
                        remote.Add(s.Replace(" remotes/", "").Trim());
                    else
                        local.Add(s.Replace("*", " ").Trim());
                }
            }
        }

        /// <summary>
        /// Switch to a named branch
        /// </summary>
        public bool SwitchTo(string name)
        {
            // Make sure the given branch name is a valid local branch
            if (!string.IsNullOrEmpty(name) && local.IndexOf(name) >= 0)
            {
                App.Git.Run("checkout " + name);
                return true;
            }
            return false;
        }
    }
}
