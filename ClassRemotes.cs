using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git4Win
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
        private Dictionary<string, Remote> _remotes = new Dictionary<string, Remote>();

        /// <summary>
        /// Return the list of names of remote repos
        /// </summary>
        public List<string> GetListNames()
        {
            List<string> list = _remotes.Select(kvp => kvp.Key).ToList();
            return list;
        }

        /// <summary>
        /// Return the remote structure associated with a given name
        /// </summary>
        public Remote Get(string name)
        {
            return _remotes[name];
        }

        /// <summary>
        /// Refresh the list of remotes for the given repo while keeping the
        /// existing passwords and push commands
        /// </summary>
        public void Refresh(ClassRepo repo)
        {
            // Build the new list while picking up password fields from the existing list
            Dictionary<string, Remote> newlist = new Dictionary<string, Remote>();

            string[] response = repo.Run("remote -v").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in response)
            {
                Remote r = new Remote();

                // Split the resulting remote repo name/url into separate strings
                string[] url = s.Split("\t ".ToCharArray());
                string name = url[0];

                // Find if the name exists in the main list and save off the password from it
                if (newlist.ContainsKey(name))
                    r = newlist[name];

                if (_remotes.ContainsKey(name))
                {
                    r.Password = _remotes[name].Password;
                    r.PushCmd = _remotes[name].PushCmd;
                }

                // Set all other fields that we refresh every time                
                r.Name = name;

                if (url[2] == "(fetch)") r.UrlFetch = url[1];
                if (url[2] == "(push)") r.UrlPush = url[1];

                // Add it to the new list
                newlist[name] = r;
            }

            // Set the newly built list to be the master list
            _remotes = newlist;

            // Fixup the new current string name
            if (!_remotes.ContainsKey(Current))
                Current = _remotes.Count > 0 ? _remotes.ElementAt(0).Key : "";
        }

        /// <summary>
        /// Sets the password field for the given remote name or
        /// creates a new remote if the named one does not exist
        /// </summary>
        public void SetPassword(string name, string password)
        {
            Remote r;
            if (!_remotes.TryGetValue(name, out r))
                r.Name = name;
            r.Password = password;
            _remotes[name] = r;
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
            _remotes.TryGetValue(name, out r);
            return r.Password;
        }

        /// <summary>
        /// Sets the push command field for the given remote name or
        /// creates a new remote if the named one does not exist
        /// </summary>
        public void SetPushCmd(string name, string cmd)
        {
            Remote r;
            if (!_remotes.TryGetValue(name, out r))
                r.Name = name;
            r.PushCmd = cmd;
            _remotes[name] = r;
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
            _remotes.TryGetValue(name, out r);
            return r.PushCmd;
        }    
    }
}
