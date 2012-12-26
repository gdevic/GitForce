using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
            { "Getting Started", "http://gdevic.github.com/GitForce/GettingStarted.html" },
            { "Edit Tools", "http://gdevic.github.com/GitForce/EditTools.html" },
            { "SSH Windows", "http://gdevic.github.com/GitForce/ssh-windows.html" },
            { "Download", "https://github.com/gdevic/GitForce/downloads" },
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