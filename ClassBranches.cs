using System;
using System.Collections.Generic;

namespace GitForce
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
        public readonly List<string> Local = new List<string>();

        /// <summary>
        /// List of remote branches by their name
        /// </summary>
        public readonly List<string> Remote = new List<string>();

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
                ExecResult result = App.Repos.Current.RunCmd("branch -a");
                if (result.Success())
                {
                    string[] response = result.stdout.Split((Environment.NewLine).ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
        }

        /// <summary>
        /// Switch to a named branch
        /// </summary>
        public bool SwitchTo(string name)
        {
            // Make sure the given branch name is a valid local branch
            if (!string.IsNullOrEmpty(name) && Local.IndexOf(name) >= 0)
            {
                ExecResult result = App.Repos.Current.RunCmd("checkout " + name);
                return result.Success();
            }
            return false;
        }
    }
}
