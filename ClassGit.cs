using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
{
    /// <summary>
    /// Functions to find git and run a git command
    /// </summary>
    class ClassGit
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
            if (ClassExecute.Run(_gitPath, "--version").Contains("git version") == false)
            {
                // If we are running on Linux, get the git path by 'which' command
                if (ClassUtils.IsMono())
                {
                    _gitPath = ClassExecute.Run("which", "git").Trim();
                    if (!ClassExecute.Run(_gitPath, "--version").Contains("git version"))
                    {
                        MessageBox.Show("Could not locate 'git'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        retValue = false;
                    }
                }
                else
                {
                    // Check if a version of git is installed at a known location
                    _gitPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Git\bin\git.exe";
                    if (ClassExecute.Run(_gitPath, "--version").Contains("git version") == false)
                    {
                        // Ask user to show us where the git is installed
                        FormPathToGit formPath = new FormPathToGit();
                        while (retValue = (formPath.ShowDialog() == DialogResult.OK))
                        {
                            _gitPath = formPath.Path;
                            if (ClassExecute.Run(_gitPath, "--version").Contains("git version"))
                                break;
                        }
                    }
                }
            }
            if (retValue)
            {
                App.Log.Print("[Git:Init] Using git at " + _gitPath);
                Properties.Settings.Default.GitPath = _gitPath;                
            }
            return retValue;
        }

        /// <summary>
        /// A generic function that executes a Git command
        /// </summary>
        public static string Run(string gitcmd)
        {
            // Pick up git commands that take long time to execute and run them
            // using a threaded execution
            if (gitcmd.StartsWith("clone --progress") || 
                gitcmd.StartsWith("pull ") || 
                gitcmd.StartsWith("push "))
            {
                FormGitRun formGitRun = new FormGitRun(Properties.Settings.Default.GitPath, gitcmd);
                formGitRun.ShowDialog();
                return "";
            }
            else
                return ClassExecute.Run(Properties.Settings.Default.GitPath, gitcmd);
        }
    }
}
