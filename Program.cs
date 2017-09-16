using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GitForce
{
    static class App
    {
        #region Application-wide delegates for various callbacks

        /// <summary>
        /// Multicast delegate for application-wide data refresh. It is initialized with the
        /// first app-global refresh handler. Other forms append their own refresh functions
        /// in the order they are instantiated.
        /// </summary>
        public delegate void RefreshDelegate();
        public static RefreshDelegate Refresh = ClassRepos.Refresh;
        private static bool inRefresh;

        /// <summary>
        /// Protect Refresh chain with a simple exit mutex, so that the F5 key (update)
        /// does not start a re-entrant refresh chain. Although the main GUI app is
        /// single-threaded, some panels refresh functions are calling GitRun() which
        /// in turn spawns external async process during which time we can end up with
        /// multiple threads trying to refresh.
        /// </summary>
        public static void DoRefresh()
        {
            if (inRefresh) return;
            inRefresh = true;
            Refresh();
            inRefresh = false;
        }

        /// <summary>
        /// Delegate to main form to print a status message in the status pane.
        /// We do it via delegate since it might be called before the main form is created.
        /// </summary>
        public delegate void PrintStatusMessageHandler(string message, MessageType type);
        public static PrintStatusMessageHandler PrintStatusMessage = VoidMessage;
        private static void VoidMessage(string m, MessageType type) { }

        /// <summary>
        /// Delegate to main form to set the busy status.
        /// We do it via delegate since it might be called before the main form is created.
        /// </summary>
        public delegate void SetBusyStatusHandler(bool isBusy);
        public static SetBusyStatusHandler StatusBusy = VoidBusy;
        private static void VoidBusy(bool f) { }

        /// <summary>
        /// Delegate to Log form to print a message in the log window.
        /// We do it via delegate since it might be called before or after log form is valid.
        /// </summary>
        public delegate void PrintLogMessageHandler(string message, MessageType type);
        public static PrintLogMessageHandler PrintLogMessage = VoidMessage;

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
        /// Static class PuTTY to manage SSL connections on Windows
        /// </summary>
        public static ClassPutty Putty;

        /// <summary>
        /// Static class SSH to manage SSL connections on Linux
        /// </summary>
        public static ClassSSH Ssh;

        /// <summary>
        /// Static class managing Git HTTPS password helper file
        /// </summary>
        public static ClassHttpsPasswd HttpsPasswd;

        /// <summary>
        /// Static class containing custom tools
        /// </summary>
        public static ClassCustomTools CustomTools;

        /// <summary>
        /// Static class containing code to check for a new version
        /// </summary>
        public static ClassVersion Version;

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
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GitForce");

        /// <summary>
        /// Define a path to the user profile.
        /// </summary>
        public static readonly string UserHome = ClassUtils.GetHomePath();

        /// <summary>
        /// If set to a file name, all log text will be mirrored to that file
        /// Command line argument '--log' sets it to application data folder, file 'gitforce.log'
        /// </summary>
        public static string AppLog;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Upgrade application settings across the version increment
            ClassUtils.UpgradeApplicationSettingsIfNecessary();

            // Make sure the application data folder directory exists
            Directory.CreateDirectory(AppHome);

            // Get and process command line arguments
            Arguments commandLine = new Arguments(args);

            // If the processing requested program termination,
            // return using the error code created by that class
            if (ClassCommandLine.Execute(commandLine) == false)
                return ClassCommandLine.ReturnCode;

            // Check that only one application instance is running
            bool mAcquired;
            Mutex mAppMutex = new Mutex(true, "gitforce", out mAcquired);
            if (!mAcquired && Properties.Settings.Default.WarnMultipleInstances)
            {
                if (MessageBox.Show("GitForce is already running.\n\nDo you want to open a new instance?", "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return -1;
            }

            // Check if the application has been run as Admin/root
            if (ClassUtils.IsAdmin())
            {
                if (MessageBox.Show("GitForce has been run with elevated privileges which is not a recomended way to run it.\n\nDo you still want to continue?", "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return -1;
            }

            // Initialize logging and git execute support
            Log = new FormLog();
            Log.ShowWindow(Properties.Settings.Default.ShowLogWindow);

            Git = new ClassGit();

            // Before we can start, we need to have a functional git executable);
            if (Git.Initialize())
            {
                // Initialize external diff program
                Diff = new ClassDiff();
                if (Diff.Initialize())
                {
                    Merge = new ClassMerge();
                    // Initialize external Merge program
                    if (Merge.Initialize())
                    {
                        // Add known text editors
                        Settings.Panels.ControlViewEdit.AddKnownEditors();

                        if (ClassUtils.IsMono())
                            Ssh = new ClassSSH();           // Instantiate SSH support only on Linux (Mono)
                        else
                            Putty = new ClassPutty();       // Instantiate PuTTY support only on Windows

                        HttpsPasswd = new ClassHttpsPasswd();// Create HTTPS password helper file
                        Repos = new ClassRepos();           // Create repository canvas
                        Version = new ClassVersion();       // Start the new version check process
                        MainForm = new FormMain();          // Create the main form
                        MainForm.Show();                    // Show the main window so the handles get created
                        if (MainForm.Initialize())          // Load repos, custom tools etc.
                        {
                            DoRefresh();                    // Initial global refresh
                            Application.Run(MainForm);      // Run the main form

                            Properties.Settings.Default.Save();

                            GC.KeepAlive(mAppMutex);
                            return 0;
                        }
                    }
                }
            }
            return -1;
        }
    }

    public enum MessageType
    {
        General,
        Command,
        Output,
        Error,
        Debug,
        NewVersion,
    }
}