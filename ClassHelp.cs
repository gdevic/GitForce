using System.Collections.Generic;
using System.Diagnostics;

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
            {"GPL", @"http://www.gnu.org/licenses/gpl.html"},
            {"BaltazarStudios", @"https://www.baltazarstudios.com" }
        };

        /// <summary>
        /// Given the topic, open the relevant help page online
        /// </summary>
        public static void Handler(string topic)
        {
            if (Webhelp.ContainsKey(topic))
            {
                if (ClassUtils.IsMono())
                    Process.Start("xdg-open", Webhelp[topic]);
                else
                    Process.Start(Webhelp[topic]);
            }
            else
            {
                App.PrintStatusMessage("Internal Error: Please report that `topic " + topic + "` not found!", MessageType.Error);
            }
        }
    }
}
