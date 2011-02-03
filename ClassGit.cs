using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    class ClassGit
    {
        /// <summary>
        /// A temporary file that provides password for HTTPS operations
        /// </summary>
        private string pathPasswordBatchHelper = Path.GetTempFileName() + ".bat";

        /// <summary>
        /// Path to git executable, saved in Properties.Settings.Default.gitPath
        /// </summary>
        private string gitPath = Properties.Settings.Default.gitPath;

        /// <summary>
        /// Need a class destructor to remove a temporary file
        /// </summary>
        ~ClassGit()
        {
            File.Delete(pathPasswordBatchHelper);
        }

        /// <summary>
        /// Checks the path to git and checks that the git executable is functional. This call
        /// should be made only once upon the program start.
        /// </summary>
        /// <returns>true if git executable can be run</returns>
        public bool Initialize()
        {
            bool retValue = true;

            // Create a temporary batch file that will provide password for HTTPS git clone operations
            File.WriteAllText(pathPasswordBatchHelper, "@echo %PASSWORD%\n");

            // Check that we have a functional version of git at an already set path
            gitPath = Properties.Settings.Default.gitPath;
            if (Run("--version").Contains("git version") == false)
            {
                // Check if a version of git is installed at a known location
                gitPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Git\bin\git.exe";
                if (Run("--version").Contains("git version") == false)
                {
                    // Ask user to show us where the git is installed
                    FormPathToGit formPath = new FormPathToGit();
                    while (retValue = formPath.ShowDialog() == DialogResult.OK)
                    {
                        gitPath = formPath.path;
                        if (Run("--version").Contains("git version") == true)
                        {
                            Properties.Settings.Default.gitPath = gitPath;
                            break;
                        }
                    }
                }
            }
            return retValue;
        }

        // Execution class provides functions to run process in a separate thread
        // Still needs debugging.
#if false
        public string Run(string args, ClassRepo repo = null, string path = null)
        {
            string password = "";
            string root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            // Show the git command that the GUI is performing if that option is checked
            if (Properties.Settings.Default.logCommands == true)
                App.Log("Executing git " + args);

            // If the repo object is not specified, use the 'current'
            if (repo == null)
                repo = App.Repos.current;

            // Before executing a git command, it is imperative to be in the correct directory
            // that is, if we are running in the context of an existing repo
            if (repo != null)
            {
                root = repo.root;
                password = repo.remotes.GetPassword();
            }

            // TODO: Fix a problem where app starts and the current repo has invalid path (was deleted)
            try
            {
                // If the path is given, it overrides any other derived path
                if (path != null)
                    Directory.SetCurrentDirectory(path);
                else
                    Directory.SetCurrentDirectory(root);
            }
            catch { }

            // Run the command
            Dictionary<string, string> env = new Dictionary<string, string>();
            env.Add("GIT_ASKPASS", pathPasswordBatchHelper);
            env.Add("PASSWORD", password);
            string[] s = App.Execute.RunThread(gitPath, args, env);

            return s[0];
        }
#else
        /// <summary>
        /// Run a git command
        /// </summary>
        /// <param name="cmd">Arguments to the git executable</param>
        /// <param name="repo">Optional repo class in which context to run the command</param>
        /// <param name="path">Optional path to execute git command</param>
        /// <returns>stdout result of running a command</returns>
        public string Run(string cmd, ClassRepo repo = null, string path = null, string password = "")
        {
            // If the repo object is not specified, use the 'current'
            if (repo == null)
                repo = App.Repos.current;

            string output = "";
            string error = "";
            string root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            // Show the git command that the GUI is performing if that option is checked
            if (Properties.Settings.Default.logCommands == true)
                App.Log("Executing git " + cmd);
            App.Execute.Add("git " + cmd);

            // Before executing a git command, it is imperative to be in the correct directory
            // that is, if we are running in the context of an existing repo
            if (repo != null)
            {
                root = repo.root;
                password = repo.remotes.GetPassword();
            }

            App.Execute.SetTitle("Command: git " + cmd);
            App.StatusBusy(true);

            try
            {
                // If the path is given, it overrides any other derived path
                if (path != null)
                    Directory.SetCurrentDirectory(path);
                else
                    Directory.SetCurrentDirectory(root);

                Process p = new Process();
                p.StartInfo.FileName = gitPath;
                p.StartInfo.EnvironmentVariables.Add("GIT_ASKPASS", pathPasswordBatchHelper);
                p.StartInfo.EnvironmentVariables.Add("PASSWORD", password);
                p.StartInfo.EnvironmentVariables.Add("GIT_SSH", App.Putty.GetPlinkPath());
                p.StartInfo.EnvironmentVariables.Add("PLINK_PROTOCOL", "ssh");
                p.StartInfo.Arguments = cmd;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();

                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();

                p.WaitForExit();

                if (output != "")
                    App.Execute.Add(output);

                if (error != "")
                    App.Log(error);
            }
            catch (Exception ex)
            {
                App.Log(ex.Message);
            }
            finally
            {
                App.Execute.SetTitle("Command log");
                App.StatusBusy(false);
            }
            return output;
        }
#endif
    }
}
