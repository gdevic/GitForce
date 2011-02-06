using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace git4win
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
        /// Delegate to call in order to print into the app-wide log class
        /// </summary>
        public delegate void LogDelegate(string message);
        public static LogDelegate Log = VoidLog;
        private static void VoidLog(string message) { }

        /// <summary>
        /// Delegate for hooking up status pane info notifications back to the main form
        /// </summary>
        public delegate void StatusInfoEventHandler(string infoMessage);
        public static StatusInfoEventHandler StatusInfo;

        /// <summary>
        /// Delegate to main form to set the busy status
        /// </summary>
        public delegate void SetBusyStatusHandler(bool isBusy);
        public static SetBusyStatusHandler StatusBusy = VoidBusy;
        private static void VoidBusy(bool f) { }

        #endregion

        #region Static forms and classes

        /// <summary>
        /// Form execute with command execution threads
        /// </summary>
        public static FormExecute Execute;

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

        #endregion

        /// <summary>
        /// Application-wide mutex preventing multiple instances
        /// </summary>
        static readonly Mutex AppMutex = new Mutex(true, "{3486D9C7-4D52-43D7-A30F-6A9B74789C5F}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (AppMutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Initialize global application static classes in this order:
                Execute = new FormExecute();
                Git = new ClassGit();
                Diff = new ClassDiff();
                Repos = new ClassRepos();
                Putty = new ClassPutty();

                // Before we can start, we need to have a functional git executable
                if (Git.Initialize())
                {
                    // Initialize external diff program
                    if (Diff.Initialize())
                    {
                        // Add known text editors if needed
                        FormOptions_Panels.ControlViewEdit.AddKnownEditors();

                        Form mainForm = new FormMain();
                        Application.Run(mainForm);

                        Properties.Settings.Default.Save();
                    }
                }
                AppMutex.ReleaseMutex();
            }
            else
            {
                // Send our NativeMethods message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WmShowme, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
