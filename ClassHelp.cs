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
        private static readonly Dictionary<string, string> Webhelp = new Dictionary<string, string> {
            { "Home", "https://sites.google.com/site/gitforcetool" },
            { "Getting Started", "https://sites.google.com/site/gitforcetool/getting-started" },
            { "Edit Tools", "https://sites.google.com/site/gitforcetool/help/custom-tools" },
            { "SSH Windows", "https://sites.google.com/site/gitforcetool/help/ssh" },
            { "Workspace", "https://sites.google.com/site/gitforcetool/help/workspaces" },
            { "Update Check", "http://sourceforge.net/projects/gitforce" },
            { "Download", "http://sourceforge.net/projects/gitforce" }
        };

        /// <summary>
        /// Given the topic, open the relevant help page online
        /// </summary>
        public static void Handler(string topic)
        {
            if(Webhelp.ContainsKey(topic))
            {
                Process.Start(Webhelp[topic]);
            }
            else
            {
                App.PrintStatusMessage("Internal Error: Please report that `topic " + topic + "` not found!", MessageType.Error);
            }
        }
    }
}