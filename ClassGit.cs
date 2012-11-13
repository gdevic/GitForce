using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Functions to find git and run a git command
    /// </summary>
    internal class ClassGit
    {
        /// <summary>
        /// Path to git executable, saved in Properties.Settings.Default.gitPath
        /// </summary>
        private string _gitPath;

        /// <summary>
        /// Checks the path to git and checks that the git executable is functional. This call
        /// should be made only once upon the program start.
        /// It returns true if git executable can be run, false otherwise.
        /// </summary>
        public bool Initialize()
        {
            bool retValue = true;

            // Check that we have a functional version of git at an already set path
            _gitPath = Properties.Settings.Default.GitPath;
            if (Exec.Run(_gitPath, "--version").stdout.Contains("git version") == false)
            {
                // If we are running on Linux, get the git path by 'which' command
                if (ClassUtils.IsMono())
                {
                    _gitPath = Exec.Run("which", "git").stdout.Trim();
                    if (Exec.Run(_gitPath, "--version").stdout.Contains("git version") == false)
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
                    string path = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)") ??
                                  Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    _gitPath = Path.Combine(path, @"Git\bin\git.exe");
                    if (Exec.Run(_gitPath, "--version").stdout.Contains("git version") == false)
                    {
                        // Ask user to show us where the git is installed
                        FormPathToGit formPath = new FormPathToGit();
                        while (retValue = (formPath.ShowDialog() == DialogResult.OK))
                        {
                            _gitPath = formPath.Path;
                            if (Exec.Run(_gitPath, "--version").stdout.Contains("git version"))
                                break;
                        }
                    }
                }
            }
            if (retValue)
            {
                // Run the version again to get the version code (for simplicity did not save it earlier)
                string version = string.Format("Using {0} at {1}", Exec.Run(_gitPath, "--version"), _gitPath);
                App.PrintLogMessage(version);
                Properties.Settings.Default.GitPath = _gitPath;
            }
            return retValue;
        }

        /// <summary>
        /// A generic function that executes a Git command
        /// </summary>
        public static ExecResult Run(string gitcmd, bool async = false)
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
            job.AsyncRun(s => App.PrintStatusMessage(s), s => App.PrintStatusMessage(s), null);
            return new ExecResult();
        }
    }
}