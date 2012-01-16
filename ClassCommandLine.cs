using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Process command line arguments
    /// </summary>
    static class ClassCommandLine
    {
        public static int ReturnCode;

        /// <summary>
        /// The main execution function for command line arguments.
        /// Returns 'false' if the main program should not continue with the execution, in which
        /// case ReturnCode contains the return code to return to the OS.
        /// </summary>
        public static bool Execute(Arguments commandLine)
        {
            // WAR: On Windows, re-attach the console so we can print. Mono does not need that to use Console class.
            if(!ClassUtils.IsMono())
            {
                NativeMethods.AttachConsole();
            }

            if (commandLine["help"] == "true")
            {
                Console.WriteLine(Environment.NewLine +
                                  "GitForce optional arguments:" + Environment.NewLine +
                                  "  --version             Show the application version number." + Environment.NewLine +
                                  "  --reset-windows       Reset stored locations of windows and dialogs." + Environment.NewLine +
                                  "  --reset-config        Reset program configuration (repos etc.)." + Environment.NewLine);
                ReturnCode = 0;
                return false;
            }

            // --version Show the application version number and quit
            if (commandLine["version"] == "true")
            {
                Console.WriteLine("GitForce version " + App.Version);

                ReturnCode = 0;
                return false;
            }

            // --reset-windows  Reset the stored locations and sizes of all internal dialogs and forms
            //                 At this time we dont reset the main window and the log window
            if (commandLine["reset-windows"] == "true")
            {
                Properties.Settings.Default.WindowsGeometries = new StringCollection();

                ReturnCode = 0;
                return false;
            }

            // --reset-config   Reset stored configuration items (repos, settings)
            if (commandLine["reset-config"] == "true")
            {
                List<string> toWhack = new List<string>();

                // This is very much dependent on the platform, load a list of directories to whack appropriately
                if(ClassUtils.IsMono())
                {
                    string home = Environment.GetEnvironmentVariable("HOME");
                    if(!string.IsNullOrEmpty(home))
                    {
                        toWhack.Add(Path.Combine(home, ".config/GitForce"));
                        toWhack.Add(Path.Combine(home, ".local/share/GitForce"));                        
                    }
                }
                else
                {
                    toWhack.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GitForce"));
                    toWhack.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GitForce"));
                }
                // Now that we have a list of directories to remove, delete them
                foreach (var dir in toWhack)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    if (ClassUtils.DeleteFolder(dirInfo, false, false) == false)
                        Console.Write("Unable to delete some files!");
                }
                Console.WriteLine("Configuration has been reset.");
                return false;
            }
            return true;
        }
    }
}
