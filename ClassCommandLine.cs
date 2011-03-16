using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
{
    /// <summary>
    /// Process command line arguments
    /// </summary>
    class ClassCommandLine
    {
        public static int ReturnCode = 0;

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
                                  "Git4Win optional arguments:" + Environment.NewLine +
                                  "  --version             Show the application version number." + Environment.NewLine +
                                  "  --reset-windows       Reset stored locations of windows and dialogs." + Environment.NewLine);
                ReturnCode = 0;
                return false;
            }

            // --version Show the application version number and quit
            if (commandLine["version"] == "true")
            {
                Console.WriteLine("Git4Win version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

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
            return true;
        }
    }
}
