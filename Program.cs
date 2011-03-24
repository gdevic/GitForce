using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GitForce
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
        /// Static class containing merge tool helpers
        /// </summary>
        public static ClassMerge Merge;

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
        /// Static class containing custom tools
        /// </summary>
        public static ClassCustomTools CustomTools;

        /// <summary>
        /// Static main form class
        /// </summary>
        public static FormMain MainForm;

        #endregion

        #region Global variables

        /// <summary>
        /// Store a path to the application executing instance
        /// </summary>
        public static readonly string AppPath = Application.ExecutablePath;

        /// <summary>
        /// Define a path to the application data folder
        /// </summary>
        public static readonly string AppHome = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GitForce");

        /// <summary>
        /// Program version number
        /// </summary>
        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return String.Format("{0}.{1}", fvi.ProductMajorPart, fvi.ProductMinorPart);
            }
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get and process command line arguments
            Arguments commandLine = new Arguments(args);

            // If the processing requested program termination,
            // return using the error code created by that class
            if (ClassCommandLine.Execute(commandLine) == false)
                return ClassCommandLine.ReturnCode;

            // Make sure the application data folder directory exists
            Directory.CreateDirectory(AppHome);
            
            // Initialize logging and git execute support
            Log = new FormLog();
            Git = new ClassGit();

            // Before we can start, we need to have a functional git executable);
            if (Git.Initialize())
            {
                // Initialize external diff program
                Diff = new ClassDiff();
                if( Diff.Initialize())
                {
                    Merge = new ClassMerge();
                    // Initialize external Merge program
                    if (Merge.Initialize())
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

                        return 0;   // Return no error code
                    }
                }
            }
            return -1;
        }
    }
}
