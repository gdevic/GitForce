using System;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Functions to find git and run a git command
    /// </summary>
    class ClassGit
    {
        /// <summary>
        /// Path to git executable, saved in Properties.Settings.Default.gitPath
        /// </summary>
        private string gitPath;

        /// <summary>
        /// Checks the path to git and checks that the git executable is functional. This call
        /// should be made only once upon the program start.
        /// It returns true if git executable can be run, false otherwise.
        /// </summary>
        public bool Initialize()
        {
            bool retValue = true;

            // Check that we have a functional version of git at an already set path
            gitPath = Properties.Settings.Default.GitPath;
            if (Exec.Run(gitPath, "--version").stdout.Contains("git version") == false)
            {
                // If we are running on Linux, get the git path by 'which' command
                if (ClassUtils.IsMono())
                {
                    gitPath = Exec.Run("which", "git").stdout.Trim();
                    if (Exec.Run(gitPath, "--version").stdout.Contains("git version") == false)
                    {
                        MessageBox.Show(
                            "Could not locate 'git'!\n\nPlease install git by running 'sudo apt-get install git'\nMake sure it is on your path, then rerun this application.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        retValue = false;
                    }
                }
                else
                {
                    // Check if a version of git is installed at a known location (or guess a location)
                    string programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)") ??
                                              Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    // If a git executable does not exist at the default location, try the 64-bit program file folder instead
                    if (!File.Exists(programFilesPath) && programFilesPath.Contains(" (x86)"))
                        programFilesPath = programFilesPath.Replace(" (x86)", "");

                    gitPath = Path.Combine(programFilesPath, @"Git\bin\git.exe");
                    if (Exec.Run(gitPath, "--version").stdout.Contains("git version") == false)
                    {
                        // Ask user to show us where the git is installed
                        FormPathToGit formPath = new FormPathToGit(programFilesPath, gitPath);
                        while (retValue = (formPath.ShowDialog() == DialogResult.OK))
                        {
                            gitPath = formPath.PathToGit;
                            if (Exec.Run(gitPath, "--version").stdout.Contains("git version"))
                                break;
                        }
                    }
                }
            }
            if (retValue)
            {
                // Run the version again to get the version code (for simplicity did not save it earlier)
                string version = string.Format("Using {0} at {1}", Exec.Run(gitPath, "--version"),gitPath);
                App.PrintLogMessage(version, MessageType.General);
                Properties.Settings.Default.GitPath = gitPath;
            }
            return retValue;
        }

        /// <summary>
        /// A generic function that executes a Git command
        /// NOTE: C# 4.0 is currently not supported on MSVC 2008
        /// </summary>
        public static ExecResult Run(string gitcmd) { return Run(gitcmd, false); }
        public static ExecResult Run(string gitcmd, bool async)
        {
            // Pick up git commands that take long time to execute and run them
            // using a threaded execution
            if (gitcmd.StartsWith("clone --progress") ||
                gitcmd.StartsWith("fetch") ||
                gitcmd.StartsWith("pull ") ||
                gitcmd.StartsWith("push "))
            {
                FormGitRun formGitRun = new FormGitRun(Properties.Settings.Default.GitPath, gitcmd);
                formGitRun.ShowDialog();
                return formGitRun.GetResult();
            }

            if (!async)
            {
                return Exec.Run(Properties.Settings.Default.GitPath, gitcmd);
            }

            var job = new Exec(Properties.Settings.Default.GitPath, gitcmd);
            job.AsyncRun(s => App.PrintStatusMessage(s, MessageType.Output),
                         s => App.PrintStatusMessage(s, MessageType.Error),
                         null);
            return new ExecResult();
        }
    }
}