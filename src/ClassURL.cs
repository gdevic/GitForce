using System;
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
        public enum UrlType { Unknown, Ssh, Git, Http, Https, Ftp, Ftps, Rsync, Local, Other };

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
        /// Parse given URL and return the type structure.
        ///
        /// Formats that parse correctly (Ok = true):
        ///
        ///   URL Format                                   Type    Host            Path                    Name
        ///   -----------------------------------------    -----   -------------   ---------------------   ----
        ///   https://github.com/user/repo.git             Https   github.com      user/repo.git           repo
        ///   https://github.com/user/repo                 Https   github.com      user/repo               repo
        ///   https://gitlab.com/group/sub/repo.git        Https   gitlab.com      group/sub/repo.git      repo
        ///   https://dev.azure.com/org/proj/_git/repo     Https   dev.azure.com   org/proj/_git/repo      repo
        ///   http://host.xz/path/repo.git                 Http    host.xz         path/repo.git           repo
        ///   ssh://git@github.com/user/repo.git           Ssh     github.com      user/repo.git           repo
        ///   ssh://git@host.xz:22/path/repo.git           Ssh     host.xz         path/repo.git           repo
        ///   ssh://host.xz/path/repo.git                  Ssh     host.xz         path/repo.git           repo
        ///   git://github.com/user/repo.git               Git     github.com      user/repo.git           repo
        ///   git://host.xz:9418/path/repo.git             Git     host.xz         path/repo.git           repo
        ///   git@github.com:user/repo.git                 Ssh     github.com      user/repo.git           repo
        ///   git@gitlab.com:group/repo.git                Ssh     gitlab.com      group/repo.git          repo
        ///   user@host.xz:path/to/repo.git                Ssh     host.xz         path/to/repo.git        repo
        ///   host.xz:path/to/repo.git                     Ssh     host.xz         path/to/repo.git        repo
        ///   ssh://git@host/~user/repo.git                Ssh     host            repo.git                repo
        ///   ftp://host.xz/path/repo.git                  Ftp     host.xz         path/repo.git           repo
        ///   rsync://host.xz/path/repo.git                Rsync   host.xz         path/repo.git           repo
        ///
        /// Addresses accepted as they stand, kept whole and never rewritten (Ok = true,
        /// Host empty, Path the address itself):
        ///
        ///   C:\Projects\repo                             Local                                           repo
        ///   /local/path/to/repo                          Local                                           repo
        ///   \\server\share\repo                          Local                                           repo
        ///   file:///path/to/repo.git                     Local                                           repo
        ///   git+ssh://git@host.xz/path/repo.git          Other                                           repo
        ///
        /// Formats that fail (Ok = false):
        ///   github.com/user/repo.git                     No scheme and no ':', so the type stays Unknown
        ///   ./relative/path                              Cannot be told apart from the line above
        ///   https://github.com                           No path
        ///   repo with spaces                             Contains spaces
        ///
        /// </summary>
        public static Url Parse(string URL)
        {
            Url url = new Url();

            url.Type = UrlType.Unknown;
            url.User = string.Empty;
            url.Host = string.Empty;
            url.Path = string.Empty;
            url.Name = string.Empty;
            url.Port = 0;
            url.Ok = false;

            // URL cannot be empty and cannot have spaces
            if (string.IsNullOrEmpty(URL))
                return url;
            string u = URL.Trim();
            if (u.Length == 0 || u.Contains(' '))
                return url;

            try
            {
                // An address on the local file system, which git reaches through the same
                // file transport whether or not it is spelled with the "file:" scheme. It has
                // to be taken here, before the ':' test below would read a drive letter as a
                // host name and the canonical form would rewrite the user's path away.
                if (IsLocalPath(u) || u.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
                    return Verbatim(url, u, UrlType.Local);

                // Some formats imply SSH, so do a quick sanity check before assigning it
                if (u.Contains(':'))
                    url.Type = UrlType.Ssh;

                // Find the easy case first
                string u1 = u;
                bool hasProtocol = false;
                foreach (var v in Protocol.Where(v => u1.StartsWith(v.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    url.Type = v.Value;
                    u = u.Substring(v.Key.Length);
                    hasProtocol = true;
                    break;
                }

                // A scheme that is not one of ours (git+ssh://, smb://, ...) is not SSH either:
                // the ':' that suggested SSH just above belongs to that scheme. We cannot take
                // such an address apart, but it is still someone's working remote, so keep it
                // whole rather than rewriting it or refusing to let the user save it.
                if (!hasProtocol && HasScheme(u))
                    return Verbatim(url, u, UrlType.Other);

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
                url.Name = ProjectName(u);

                // Final check that we have at least necessary portions of the address.
                // A recognized type is required as well: with neither a protocol prefix nor
                // a ':' there is nothing here that git would treat as a remote URL, and
                // ToCanonical has no protocol or default port to rebuild such an address with.
                if (url.Type != UrlType.Unknown && !string.IsNullOrEmpty(url.Host) && !string.IsNullOrEmpty(url.Path))
                    url.Ok = true;
            }
            catch
            {}

            return url;
        }

        /// <summary>
        /// Fills in an address that we accept but deliberately do not take apart, so that it
        /// survives a round trip through the editor exactly as the user wrote it
        /// </summary>
        private static Url Verbatim(Url url, string u, UrlType type)
        {
            url.Type = type;
            url.Path = u;
            url.Name = ProjectName(u);
            url.Ok = true;
            return url;
        }

        /// <summary>
        /// Returns true if the address is a path on the local file system rather than a
        /// remote URL. Two shapes qualify: a rooted path, which covers UNC names as well,
        /// and a Windows drive prefix. Git draws the same line, and on Windows it reads
        /// "C:\repo" as a drive-qualified path and never as the SSH host "C".
        /// A relative path is deliberately not included: it cannot be told apart from the
        /// scheme-less "host.xz/path" form, which this parser rejects.
        /// </summary>
        private static bool IsLocalPath(string u)
        {
            if (u[0] == '/' || u[0] == '\\')
                return true;

            // The drive prefix is a Windows shape only: on Unix "c:/path" is an SSH address.
            // Git tests for a single letter the same way, so "C:repo" is a path here as well.
            return !ClassUtils.IsMono() && u.Length > 2 && u[1] == ':' && char.IsLetter(u[0]);
        }

        /// <summary>
        /// Returns true if the address opens with a URL scheme, meaning "name://". The test is
        /// anchored: a "://" that turns up later belongs to the path, as in the mirror address
        /// "git@host.xz:mirror/https://x/repo.git", and does not make this a scheme.
        /// </summary>
        private static bool HasScheme(string u)
        {
            int i = u.IndexOf("://", StringComparison.Ordinal);
            if (i < 1 || !char.IsLetter(u[0]))
                return false;

            for (int k = 1; k < i; k++)
                if (!char.IsLetterOrDigit(u[k]) && u[k] != '+' && u[k] != '-' && u[k] != '.')
                    return false;
            return true;
        }

        /// <summary>
        /// Returns the project name portion of a repo address: the last segment, or the one
        /// before it when the address ends in the ".git" or ".code" suffix. The ':' counts as
        /// a separator so that a drive-relative path such as "C:repo" still names "repo".
        /// </summary>
        private static string ProjectName(string path)
        {
            string[] tokens = path.Split(new[] { '\\', '/', '.', ':' });
            if (tokens.Length >= 2 && (tokens[tokens.Length - 1].ToLower() == "git" || tokens[tokens.Length - 1].ToLower() == "code"))
                return tokens[tokens.Length - 2];
            return tokens[tokens.Length - 1];
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

            // Only an address whose type the protocol table names can be rebuilt. Anything
            // else, whether unparsed or accepted as it stands, has no protocol prefix and no
            // default port, so hand back what the caller gave us rather than an empty string
            // or an exception. Callers write this result into git, so it has to stay usable.
            if (!url.Ok || !Protocol.ContainsValue(url.Type))
                return URL == null ? string.Empty : URL.Trim();

            StringBuilder canon = new StringBuilder();

            // Reverse lookup the protocol type dictionary
            string proto = (from kvp in Protocol where kvp.Value == url.Type select kvp.Key).ToArray().First();
            canon.Append(proto);

            // USERNAME is the Windows spelling and is not set under Mono, where the
            // variable to read is USER. Test for null as well: the account name comes back
            // null rather than empty when the variable is missing, and the "anonymous"
            // fallback below never ran, leaving addresses of the form "ssh://@host".
            if (string.IsNullOrEmpty(url.User))
                url.User = Environment.GetEnvironmentVariable("USERNAME");
            if (string.IsNullOrEmpty(url.User))
                url.User = Environment.GetEnvironmentVariable("USER");
            if (string.IsNullOrEmpty(url.User))
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
