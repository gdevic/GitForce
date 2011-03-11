using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Git4Win
{
    static class App
    {
        #region Application-wide delegates for various callbacks

        /// <summary>
        /// Multicast delegate for application-wide data refresh
        /// </summary>
        public delegate void RefreshDelegate();
        public static RefreshDelegate Refresh = VoidRefresh;
        private static void VoidRefresh() { }

        /// <summary>
        /// Delegate to main form to print a status message in the status pane.
        /// We do it via delegate since it might be called before the main form is created.
        /// </summary>
        public delegate void PrintStatusMessageHandler(string message);
        public static PrintStatusMessageHandler PrintStatusMessage = VoidMessage;
        private static void VoidMessage(string m) { }

        /// <summary>
        /// Delegate to main form to set the busy status.
        /// We do it via delegate since it might be called before the main form is created.
        /// </summary>
        public delegate void SetBusyStatusHandler(bool isBusy);
        public static SetBusyStatusHandler StatusBusy = VoidBusy;
        private static void VoidBusy(bool f) { }

        #endregion

        #region Static forms and classes

        /// <summary>
        /// Static form with log output
        /// </summary>
        public static FormLog Log;

        /// <summary>
        /// Static git class helper containing git-execution services
        /// </summary>
        public static ClassGit Git;

        /// <summary>
        /// Static class containing diff tool execution helpers
        /// </summary>
        public static ClassDiff Diff;

        /// <summary>
        /// Static class of repos containing operations on a set of repositories
        /// </summary>
        public static ClassRepos Repos;

        /// <summary>
        /// Static class PuTTY to manage SSL connections
        /// </summary>
        public static ClassPutty Putty;

        /// <summary>
        /// Static class managing Git HTTPS password helper file
        /// </summary>
        public static ClassGitPasswd GitPasswd;

        /// <summary>
        /// Static main form class
        /// </summary>
        public static FormMain MainForm;

        #endregion

        #region Global variables

        /// <summary>
        /// Define a path to the application data folder
        /// </summary>
        public static string AppHome = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Git4Win");

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Arguments commandLine = new Arguments(args);

            if (commandLine["help"] == "true")
            {
                Console.WriteLine("Git4Win optional arguments:");
                Console.WriteLine("  --version             Show the application version number.");
                Console.WriteLine("  --reset-windows       Reset stored locations of windows and dialogs.");

                return -1;
            }

            // --version Show the application version number and quit
            if (commandLine["version"] == "true")
            {
                Console.WriteLine("Git4Win version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                return -1;
            }

            // --reset-windows Reset the stored locations and sizes of all internal dialogs and forms
            //                 At this time we dont reset the main window and the log window
            if (commandLine["reset-windows"]=="true")
                Properties.Settings.Default.WindowsGeometries = new StringCollection();


            // Make sure the application data folder directory exists
            Directory.CreateDirectory(AppHome);
            
            // Initialize logging and git execute support
            Log = new FormLog();
            Git = new ClassGit();

            // Before we can start, we need to have a functional git executable);
            if (Git.Initialize())
            {
                Diff = new ClassDiff();
                // Initialize external diff program
                if( Diff.Initialize())
                {
                    // Add known text editors
                    Settings.Panels.ControlViewEdit.AddKnownEditors();

                    // Instantiate PuTTY support only on Windows OS
                    if (!ClassUtils.IsMono())
                        Putty = new ClassPutty();

                    // Create HTTPS password helper file
                    GitPasswd = new ClassGitPasswd();

                    Repos = new ClassRepos();

                    MainForm = new FormMain();
                    Application.Run(MainForm);

                    Properties.Settings.Default.Save();                        
                }
            }
            return 0;
        }
    }
}
