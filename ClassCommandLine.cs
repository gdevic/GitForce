﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Process command line arguments
    /// </summary>
    static class ClassCommandLine
    {
        public static int ReturnCode = -1;
        public static string initRepo = null;
        private static bool runGitForce = true;

        /// <summary>
        /// The main execution function for command line arguments.
        /// Returns 'false' if the main program should not continue with the execution, in which
        /// case ReturnCode contains the return code to return to the OS.
        /// </summary>
        public static bool Execute(Arguments commandLine)
        {
            // WAR: On Windows, re-attach the console so we can print. Mono does not need that to use Console class.
            if (!ClassUtils.IsMono())
            {
                NativeMethods.AttachConsole();
                Console.WriteLine(Environment.NewLine);
            }

            if (commandLine["help"] == "true" || commandLine["?"] == "true")
            {
                Console.WriteLine(Environment.NewLine +
                                  "GitForce optional arguments:" + Environment.NewLine +
                                  "  --version             Show the application version number." + Environment.NewLine +
                                  "  --reset-windows       Reset stored locations of windows and dialogs." + Environment.NewLine +
                                  "  --reset-config        Reset program configuration (repos etc.)." + Environment.NewLine +
                                  "  --repo=\"<path>\"       Open a specified existing git repo upon start." + Environment.NewLine +
                                  "  --log                 Logs debug output to file." + Environment.NewLine);
                ReturnCode = 0;
                runGitForce = false;
            }

            // --version Show the application version number and quit
            if (commandLine["version"] == "true")
            {
                Console.WriteLine("GitForce version " + ClassVersion.GetVersion());
                ReturnCode = 0;
                runGitForce = false;
            }

            // --reset-windows  Reset stored locations and sizes of all windows and dialogs
            if (commandLine["reset-windows"] == "true")
            {
                Properties.Settings.Default.WindowsGeometries = new StringCollection();
                Properties.Settings.Default.Save();
                Console.WriteLine("GitForce windows and dialogs geometries have been reset.");
                ReturnCode = 0;
                runGitForce = true;
            }

            // --reset-config   Reset stored configuration items (repos, settings)
            if (commandLine["reset-config"] == "true")
            {
                List<string> toWhack = new List<string>();

                // This is very much dependent on the platform, load a list of directories to whack appropriately
                if (ClassUtils.IsMono())
                {
                    string home = Environment.GetEnvironmentVariable("HOME");
                    if (!string.IsNullOrEmpty(home))
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
                ReturnCode = 0;
                runGitForce = false;
            }

            // --log   Create a log file and append all debug log messages to it
            if (commandLine["log"] == "true")
            {
                App.AppLog = Path.Combine(App.AppHome, "gitforce.log");
                File.WriteAllText(App.AppLog, "Log created on " + DateTime.Now.ToShortDateString() + Environment.NewLine);
                Console.WriteLine("Logging: " + App.AppLog);
            }

            // --passwd  This is not a user option. It is used when the app is called to provide password echoed on a command line.
            if (commandLine["passwd"] == "true")
            {
                ReturnCode = -1;
                FormHttpsAuth httpsAuth = new FormHttpsAuth(false);
                if (httpsAuth.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine(httpsAuth.Password);
                    ReturnCode = 0;
                }
                runGitForce = false;
            }

            // --repo="<path>"  Open an existing git repo at the specified path
            if (commandLine["repo"] == "true") // --repo requires an additional argument
            {
                Console.WriteLine("Malformed arguments");
                ReturnCode = -1;
                runGitForce = false;
            }
            else if (commandLine["repo"] != null)
            {
                initRepo = commandLine["repo"];
                // Substitute home folder for the token ~/
                if (initRepo.StartsWith("~/"))
                    initRepo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), initRepo.Substring(2));
                initRepo = Path.GetFullPath(initRepo.TrimEnd(new[] { '/', '\\' })); // Further correct the path formatting if needed
                string initGitRepo = Path.Combine(initRepo, ".git");
                if (Directory.Exists(initGitRepo))
                    Console.WriteLine("Open git repo " + initRepo);
                else
                {
                    Console.WriteLine("The specified folder does not contain a valid git repo: " + initRepo);
                    ReturnCode = -1;
                    runGitForce = false;
                }
            }

            // WAR: On Windows, detach the console when we are done. Mono does not need that to use Console class.
            if (!ClassUtils.IsMono())
                NativeMethods.FreeConsole();

            return runGitForce;
        }
    }
}
