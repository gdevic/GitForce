using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git4Win
{
    /// <summary>
    /// List of branches for the current repo, and a set of functions to operate on them
    /// </summary>
    [Serializable]
    public class ClassBranches
    {
        /// <summary>
        /// Name of the current branch
        /// </summary>
        public string Current;

        /// <summary>
        /// List of local branches by their name
        /// </summary>
        public List<string> Local = new List<string>();

        /// <summary>
        /// List of remote branches by their name
        /// </summary>
        public List<string> Remote = new List<string>();

        /// <summary>
        /// Refresh the list of branches and assign local and remote
        /// </summary>
        public void Refresh()
        {
            Local.Clear();
            Remote.Clear();
            Current = null;

            if (App.Repos.Current != null)
            {
                string[] response = App.Repos.Current.Run("branch -a").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in response)
                {
                    // Recognize current branch - it is marked by an asterisk
                    if (s[0] == '*')
                        Current = s.Replace("*", " ").Trim();

                    // Detect if a branch is local or remote and add it to the appropriate list
                    if (s.Contains(" remotes/"))
                        Remote.Add(s.Replace(" remotes/", "").Trim());
                    else
                        Local.Add(s.Replace("*", " ").Trim());
                }
            }
        }

        /// <summary>
        /// Switch to a named branch
        /// </summary>
        public bool SwitchTo(string name)
        {
            // Make sure the given branch name is a valid local branch
            if (!string.IsNullOrEmpty(name) && Local.IndexOf(name) >= 0)
            {
                App.Repos.Current.Run("checkout " + name);
                return true;
            }
            return false;
        }
    }
}
