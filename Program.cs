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
        public static RefreshDelegate Refresh = App.voidRefresh;
        private static void voidRefresh() { }

        /// <summary>
        /// Delegate to call in order to print into the app-wide log class
        /// </summary>
        public delegate void LogDelegate(string message);
        public static LogDelegate Log = voidLog;
        private static void voidLog(string message) { }

        /// <summary>
        /// Delegate for hooking up status pane info notifications back to the main form
        /// </summary>
        public delegate void StatusInfoEventHandler(string infoMessage);
        public static StatusInfoEventHandler StatusInfo;

        /// <summary>
        /// Delegate to main form to set the busy status
        /// </summary>
        public delegate void SetBusyStatusHandler(bool isBusy);
        public static SetBusyStatusHandler StatusBusy = voidBusy;
        private static void voidBusy(bool f) { }

        #endregion

        #region Static forms and classes

        /// <summary>
        /// Form execute with command execution threads
        /// </summary>
        public static FormExecute Execute = null;

        /// <summary>
        /// Static git class helper containing git-execution services
        /// </summary>
        public static ClassGit Git = null;

        /// <summary>
        /// Static class containing diff tool execution helpers
        /// </summary>
        public static ClassDiff Diff = new ClassDiff();

        /// <summary>
        /// Static class of repos containing operations on a set of repositories
        /// </summary>
        public static ClassRepos Repos = new ClassRepos();

        /// <summary>
        /// Static class PuTTY to manage SSL connections
        /// </summary>
        public static ClassPutty Putty = new ClassPutty();

        #endregion

        /// <summary>
        /// Application-wide mutex preventing multiple instances
        /// </summary>
        static Mutex appMutex = new Mutex(true, "{3486D9C7-4D52-43D7-A30F-6A9B74789C5F}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (appMutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Before opening the main form, make sure we have a functional git executable
                Execute = new FormExecute();
                Git = new ClassGit();
                if (Git.Initialize() == true)
                {
                    // Initialize external diff program
                    if (Diff.Initialize() == true)
                    {
                        // Add known text editors if needed
                        git4win.FormOptions_Panels.ControlViewEdit.AddKnownEditors();

                        Form MainForm = new FormMain();
                        Application.Run(MainForm);

                        Properties.Settings.Default.Save();
                    }
                }
                appMutex.ReleaseMutex();
            }
            else
            {
                // Send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                ClassWin32.PostMessage((IntPtr)ClassWin32.HWND_BROADCAST, ClassWin32.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
