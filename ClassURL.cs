using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitForce
{
    /// <summary>
    /// Static helper class that parses URLs into various protocol variations
    /// that git understands
    /// </summary>
    public static class ClassUrl
    {
        /// <summary>
        /// Type of the URL address
        /// </summary>
        public enum UrlType { Unknown, Ssh, Git, Http, Https, Ftp, Ftps, Rsync };

        private static readonly Dictionary<string, UrlType> Protocol = new Dictionary<string, UrlType> {
            { "ssh://", UrlType.Ssh },
            { "git://", UrlType.Git },
            { "http://", UrlType.Http },
            { "https://", UrlType.Https },
            { "ftp://", UrlType.Ftp },
            { "ftps://", UrlType.Ftps },
            { "rsync://", UrlType.Rsync }
        };

        /// <summary>
        /// Structure containing parsed URL information
        /// </summary>
        public struct Url
        {
            public bool Ok;         // Is the path correctly parsed?
            public UrlType Type;    // Type of the URL address
            public string User;     // User name in 'user@host.xz'
            public string Host;     // Host name in 'user@host.xz'
            public string Path;     // Path to git repo as in '/path/to/repo.git'
            public string Name;     // Project name portion only as in 'repo'
            public uint Port;       // Port that is specified or 0 (valid SSH port is never 0)
        }

        /// <summary>
        /// Parse given URL and return the type structure
        /// </summary>
        public static Url Parse(string URL)
        {
            string u = URL.Trim();
            Url url = new Url();

            url.Type = UrlType.Unknown;
            url.User = string.Empty;
            url.Host = string.Empty;
            url.Path = string.Empty;
            url.Name = string.Empty;
            url.Port = 0;
            url.Ok = false;

            try
            {
                if (string.IsNullOrEmpty(u))
                    throw new ArgumentNullException();

                // Some formats imply SSH, so do a quick sanity check before assigning it
                if (u.Contains(':'))
                    url.Type = UrlType.Ssh;

                // Find the easy case first
                string u1 = u;
                foreach (var v in Protocol.Where(v => u1.StartsWith(v.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    url.Type = v.Value;
                    u = u.Substring(v.Key.Length);
                    break;
                }

                // The next one to appear could be the user@
                if (u.Contains('@'))
                {
                    url.User = u.Substring(0, u.IndexOf('@'));
                    u = u.Substring(u.IndexOf('@') + 1);
                }

                // The host name follows, terminated by [:port] or /path
                if (u.Contains(':'))
                {
                    url.Host = u.Substring(0, u.IndexOf(':'));
                    u = u.Substring(u.IndexOf(':') + 1);
                }
                else
                    if (u.Contains('/'))
                    {
                        url.Host = u.Substring(0, u.IndexOf('/'));
                        u = u.Substring(u.IndexOf('/') + 1);
                    }

                // What comes next might be a port number or might not
                if (uint.TryParse(u.Split('/').First(), out url.Port))
                {
                    if (u.IndexOf('/') > 0)
                        u = u.Substring(u.IndexOf('/') + 1);
                }

                // Trim possible slash at this point
                u = u.TrimStart('/');

                // The ~ specifies the user at this point
                if (u.Length > 0 && u[0] == '~')
                {
                    if (u.IndexOf('/') > 0)
                    {
                        url.User = u.Substring(1, u.IndexOf('/') - 1);
                        u = u.Substring(u.IndexOf('/') + 1);
                    }
                }

                // The rest is the path to git repo
                u = u.TrimStart('/');
                url.Path = u;

                // Find the project name
                string[] tokens = u.Split(new[] { '\\', '/', '.' });
                if (tokens.Length >= 2 && tokens[tokens.Length - 1].ToLower() == "git")
                    url.Name = tokens[tokens.Length - 2];
                else
                    url.Name = tokens[tokens.Length - 1];

                // Final check that we have at least necessary portions of the address
                if (!string.IsNullOrEmpty(url.Host) && !string.IsNullOrEmpty(url.Path))
                    url.Ok = true;
            }
            catch
            {}

            return url;
        }

        /// <summary>
        /// Parses an URL string and reassembles it using canonical values
        /// This function is really added to fix the ssh strings when using plink where the
        /// format of the remote has to be formalized (for instance, it has to have a user name
        /// specified etc.)
        /// </summary>
        public static string ToCanonical(string URL)
        {
            Url url = Parse(URL);
            StringBuilder canon = new StringBuilder();
            if (url.Ok)
            {
                // Reverse lookup the protocol type dictionary
                string proto = (from kvp in Protocol where kvp.Value == url.Type select kvp.Key).ToArray().First();
                canon.Append(proto);

                if (url.User == string.Empty)
                    url.User = Environment.GetEnvironmentVariable("USERNAME");
                if (url.User == string.Empty)
                    url.User = "anonymous";
                canon.Append(url.User);
                canon.Append("@" + url.Host);

                // Default port mappings from: http://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers
                Dictionary<UrlType, string> port = new Dictionary<UrlType, string> {
                    { UrlType.Ssh,  ":22" },
                    { UrlType.Git,  ":9418" },
                    { UrlType.Http, ":80" },
                    { UrlType.Https,":443" },
                    { UrlType.Ftp,  ":20" },
                    { UrlType.Ftps, ":989" },
                    { UrlType.Rsync,":873"  }
                };
                if (url.Port == 0)
                    canon.Append(port[url.Type]);
                else
                    canon.Append(":" + url.Port);

                canon.Append("/" + url.Path);
            }

            return canon.ToString();
        }

#if false
        public static void Test()
        {
            string canon;
            Url url;

            url = Parse("ssh://user@host.xz:22/path/to/repo.git/");
            url = Parse("ssh://host.xz:22/path/to/repo.git/");
            url = Parse("ssh://user@host.xz/path/to/repo.git/");
            url = Parse("ssh://host.xz/path/to/repo.git/");

            url = Parse("git://host.xz:22/path/to/repo.git/");

            url = Parse("user@host.xz:path/to/repo.git/");
            url = Parse("host.xz:path/to/repo.git/");

            url = Parse("ssh://user@host.xz:22/~user/path/to/repo.git/");
            url = Parse("ssh://host.xz/~user/path/to/repo.git/");

            url = Parse("git://host.xz:22/~user/path/to/repo.git/");
            url = Parse("git://host.xz/~user/path/to/repo.git/");

            url = Parse("user@host.xz:/~user/path/to/repo.git/");

            url = Parse("git@github.com:gdevic/MySpiEeprom.git");

            canon = ToCanonical("ssh://user@host.xz:22/path/to/repo.git/");
            canon = ToCanonical("ssh://host.xz:22/path/to/repo.git/");
            canon = ToCanonical("ssh://user@host.xz/path/to/repo.git/");
            canon = ToCanonical("ssh://host.xz/path/to/repo.git/");

            canon = ToCanonical("git://host.xz:22/path/to/repo.git/");

            canon = ToCanonical("user@host.xz:path/to/repo.git/");
            canon = ToCanonical("host.xz:path/to/repo.git/");

            canon = ToCanonical("ssh://user@host.xz:22/~user/path/to/repo.git/");
            canon = ToCanonical("ssh://host.xz/~user/path/to/repo.git/");

            canon = ToCanonical("git://host.xz:22/~user/path/to/repo.git/");
            canon = ToCanonical("git://host.xz/~user/path/to/repo.git/");

            canon = ToCanonical("user@host.xz:/~user/path/to/repo.git/");

            canon = ToCanonical("git@github.com:gdevic/MySpiEeprom.git");        
        }
#endif
    }
}
