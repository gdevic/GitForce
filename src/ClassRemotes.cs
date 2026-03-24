using System;
using System.Collections.Generic;
using System.Linq;

namespace GitForce
{
    /// <summary>
    /// Class containing a set of remotes for a given repository
    /// This class mainly manages names, command alias and passwords - things that
    /// are not kept natively with a git repo
    /// </summary>
    [Serializable]
    public class ClassRemotes
    {
        /// <summary>
        /// Structure describing a remote repo. Only the name, push cmd and password are
        /// to be used across the sessions, while the URL fields gets rewritten
        /// every time the list of remotes is updated from git
        /// </summary>
        [Serializable]
        public struct Remote
        {
            public string Name;
            public string UrlFetch;
            public string UrlPush;
            public string PushCmd;
            public string Password;
        }

        /// <summary>
        /// Current (default) remote name
        /// </summary>
        public string Current = "";

        /// <summary>
        /// Stores the current list of remotes and serves as a
        /// lookup dictionary of passwords for a given remote name
        /// </summary>
        private Dictionary<string, Remote> remotes = new Dictionary<string, Remote>();

        /// <summary>
        /// Return the list of names of remote repos
        /// </summary>
        public List<string> GetListNames()
        {
            List<string> list = remotes.Select(kvp => kvp.Key).ToList();
            return list;
        }

        /// <summary>
        /// Return the remote structure associated with a given name
        /// If the name is not found, return an empty Remote structure
        /// </summary>
        public Remote Get(string name)
        {
            return remotes.ContainsKey(name) ? remotes[name] : new Remote();
        }

        /// <summary>
        /// Return a list of remote names
        /// </summary>
        public List<string> GetRemoteNames()
        {
            return remotes.Keys.ToList();
        }

        /// <summary>
        /// Refresh the list of remotes for the given repo while keeping the
        /// existing passwords and push commands
        /// </summary>
        public void Refresh(ClassRepo repo)
        {
            // Build the new list while picking up password fields from the existing list
            Dictionary<string, Remote> newlist = new Dictionary<string, Remote>();

            string[] response = new[] {string.Empty};
            ExecResult result = repo.Run("remote -v");
            if (result.Success())
            {
                response = result.stdout.Split((Environment.NewLine).ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in response)
                {
                    Remote r = new Remote();

                    // Split the resulting remote repo name/url into separate strings
                    string[] url = s.Split("\t ".ToCharArray());
                    string name = url[0];

                    // Find if the name exists in the main list and save off the password from it
                    if (newlist.ContainsKey(name))
                        r = newlist[name];

                    if (remotes.ContainsKey(name))
                    {
                        r.Password = remotes[name].Password;
                        r.PushCmd = remotes[name].PushCmd;
                    }

                    // Set all other fields that we refresh every time
                    r.Name = name;

                    if (url[2] == "(fetch)") r.UrlFetch = url[1];
                    if (url[2] == "(push)") r.UrlPush = url[1];

                    // Add it to the new list
                    newlist[name] = r;
                }
            }

            // Set the newly built list to be the master list
            remotes = newlist;

            // Fixup the new current string name
            if (!remotes.ContainsKey(Current))
                Current = remotes.Count > 0 ? remotes.ElementAt(0).Key : "";
        }

        /// <summary>
        /// Sets the password field for the given remote name or
        /// creates a new remote if the named one does not exist
        /// </summary>
        public void SetPassword(string name, string password)
        {
            Remote r;
            if (!remotes.TryGetValue(name, out r))
                r.Name = name;
            r.Password = password;
            remotes[name] = r;
        }

        /// <summary>
        /// Return the password for a given remote by name or
        /// the current remote (if name is empty string)
        /// </summary>
        public string GetPassword(string name)
        {
            Remote r;
            r.Password = "";
            if (name == "") name = Current;
            remotes.TryGetValue(name, out r);
            return r.Password;
        }

        /// <summary>
        /// Sets the push command field for the given remote name or
        /// creates a new remote if the named one does not exist
        /// </summary>
        public void SetPushCmd(string name, string cmd)
        {
            Remote r;
            if (!remotes.TryGetValue(name, out r))
                r.Name = name;
            r.PushCmd = cmd;
            remotes[name] = r;
        }

        /// <summary>
        /// Return the push cmd for a given remote by name or
        /// the current remote (if name is empty string)
        /// </summary>
        public string GetPushCmd(string name)
        {
            Remote r;
            r.PushCmd = "";
            if (name == "") name = Current;
            remotes.TryGetValue(name, out r);
            return r.PushCmd;
        }
    }
}
