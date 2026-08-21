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
                    // Each line reads "<name>\t<url> (fetch)" or "<name>\t<url> (push)". Split on
                    // the first tab only: splitting on spaces as well loses the url of any remote
                    // whose path contains one, which a local path remote easily does, and that
                    // remote then silently ends up with no url and no way to fetch or push.
                    int tab = s.IndexOf('\t');
                    if (tab <= 0)
                        continue;               // Not a line we recognize, skip it
                    string name = s.Substring(0, tab);
                    string url = s.Substring(tab + 1).Trim();

                    // The trailing marker says which of the two urls this line carries. Strip it
                    // searching from the end, so a url holding spaces or brackets stays intact.
                    bool isFetch = url.EndsWith("(fetch)", StringComparison.Ordinal);
                    bool isPush = url.EndsWith("(push)", StringComparison.Ordinal);
                    if (isFetch || isPush)
                    {
                        int marker = url.LastIndexOf(' ');
                        url = marker > 0 ? url.Substring(0, marker).Trim() : string.Empty;
                    }

                    Remote r = new Remote();

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

                    // A line carrying no marker is the fetch line of a remote that has no fetch
                    // url. Current git prints "name<TAB>" both for a remote whose url is unset
                    // and for one that only holds a pushurl, where the (push) line follows it.
                    // So anything that is not explicitly a push line is the fetch url. Assigning
                    // such a line to both fields would let a url that got split across lines
                    // enable pushing to a truncated address.
                    if (isPush)
                        r.UrlPush = url;
                    else
                        r.UrlFetch = url;

                    // Add it to the new list
                    newlist[name] = r;
                }

                // Set the newly built list to be the master list
                remotes = newlist;

                // Fixup the new current string name
                if (!remotes.ContainsKey(Current))
                    Current = remotes.Count > 0 ? remotes.ElementAt(0).Key : "";
            }
            else
            {
                // Keep the list we already have. Replacing it with the empty one built above
                // would discard the passwords and push commands held for these remotes, which
                // are the only fields in this class meant to outlive the session, and this
                // command fails for reasons that are usually temporary: a bad line in
                // .git/config, or a repo folder that is momentarily unreachable.
                App.PrintLogMessage("Remotes refresh skipped for " + repo.Path + ": " + result.stderr, MessageType.Error);
            }
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
            // Coalesce on the way out rather than pre-setting a field: TryGetValue overwrites the
            // whole struct with default(Remote) when the key is absent, so anything assigned
            // before the call is lost and the caller would be handed a null back
            Remote r;
            if (name == "") name = Current;
            return remotes.TryGetValue(name, out r) ? (r.Password ?? "") : "";
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
            // See GetPassword above for why the coalesce has to happen after TryGetValue
            Remote r;
            if (name == "") name = Current;
            return remotes.TryGetValue(name, out r) ? (r.PushCmd ?? "") : "";
        }
    }
}
