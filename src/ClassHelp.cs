using System.Collections.Generic;

namespace GitForce
{
    /// <summary>
    /// Contains the code to handle various help links and access to the online help
    /// </summary>
    static class ClassHelp
    {
        /// <summary>
        /// Dictionary containing the translation of help topics to the web pages
        /// </summary>
        private static readonly Dictionary<string, string> Webhelp = new Dictionary<string, string>
        {
            {"Home", @"https://baltazarstudios.com/software/gitforce/"},
            {"Getting Started", @"https://baltazarstudios.com/software/gitforce#getting-started"},
            {"Edit Tools", @"https://baltazarstudios.com/software/gitforce-tools"},
            {"HTTPS Authentication", @"https://confluence.atlassian.com/fisheye/permanent-authentication-for-git-repositories-over-http-s-298977121.html"},
            {"SSH Windows", @"https://baltazarstudios.com/software/gitforce-ssh"},
            {"Workspace", @"https://baltazarstudios.com/software/gitforce-workspaces"},
            {"Update Check", @"https://github.com/gdevic/GitForce/releases"},
            {"Download", @"https://github.com/gdevic/GitForce/releases"},
            {"Discussion", @"https://sourceforge.net/p/gitforce/discussion"},
            {"GPLv3", @"https://www.gnu.org/licenses/gpl-3.0.en.html"},
            {"BaltazarStudios", @"https://www.baltazarstudios.com" }
        };

        /// <summary>
        /// Given the topic, open the relevant help page online
        /// </summary>
        public static void Handler(string topic)
        {
            if (!Webhelp.ContainsKey(topic))
            {
                App.PrintStatusMessage("Internal Error: Please report that `topic " + topic + "` not found!", MessageType.Error);
                return;
            }
            // Hand this to the shared helper rather than starting a browser here: it knows how
            // to launch one on each platform, and it reports a failure instead of throwing.
            // Launching a browser can always fail, so this must never escape to the caller.
            ClassUtils.OpenWebLink(Webhelp[topic]);
        }
    }
}
