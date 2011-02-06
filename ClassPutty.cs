using System;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace git4win
{
    /// <summary>
    /// Manage PuTTY, PLink and PuttyGen utilities
    /// </summary>
    public class ClassPutty
    {
        private readonly string _pathPageant;
        private readonly string _pathPlink;
        private readonly string _pathPuttyGen;
        private Process _procPageant = new Process();

        /// <summary>
        /// Constructor class function, create executables in temp space
        /// </summary>
        public ClassPutty()
        {
            _pathPageant = WriteResourceToFile(Properties.Resources.pageant, "pageant.exe");
            _pathPlink = WriteResourceToFile(Properties.Resources.plink, "plink.exe");
            _pathPuttyGen = WriteResourceToFile(Properties.Resources.puttygen, "puttygen.exe");

            // Run the daemon process, update keys
            RunPageantUpdateKeys();
        }

        /// <summary>
        /// Destructor for the class make sure the executable resources are properly disposed of
        /// </summary>
        ~ClassPutty()
        {
            // No real harm done if we fail to remove these temp files. Next time git4win is
            // run we will reuse the same file names, so the junk will not grow in the temp folder.
            try
            {
                // Dont attempt to stop/remove Pageant if the user wanted to leave it running
                if (Properties.Settings.Default.leavePageant == false)
                {
                    if (!_procPageant.HasExited)
                    {
                        _procPageant.Kill();
                        _procPageant.WaitForExit();
                    }
                    File.Delete(_pathPageant);
                }

                File.Delete(_pathPlink);
                File.Delete(_pathPuttyGen);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks the list of running processes in the system and returns true if a named process is running
        /// </summary>
        private static bool IsProcessRunning(string name)
        {
            return Process.GetProcesses().Any(proc => proc.ProcessName == name);
        }

        /// <summary>
        /// Writes binary resource to a temporary file
        /// </summary>
        private static string WriteResourceToFile(byte[] buffer, string filename)
        {
            string path = Path.Combine(Path.GetTempPath(), filename);
            try
            {
                using (var sw = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
            }
            catch(Exception ex)
            {
                App.Execute.Add(ex.Message);
            }
            return path;
        }

        /// <summary>
        /// Run the PuTTYgen process and wait until it exits.
        /// </summary>
        public void RunPuTTYgen()
        {
            Process procPuTTY = Process.Start(_pathPuttyGen);

            // Block until PuTTY process closes. This is safer than dealing with
            // various combinations of users starting multiple instances etc.
            procPuTTY.WaitForExit();
        }

        /// <summary>
        /// Runs the pageant daemon process and loads keys
        /// </summary>
        public void RunPageantUpdateKeys()
        {
            // Load list of keys and passphrases to send to the pageant process
            List<string> keys = Properties.Settings.Default.PuTTYKeys.
                Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> pfs = GetPassPhrases();

            StringBuilder args = new StringBuilder();

            // Passphrases must be specified first in the list of arguments
            foreach (string s in pfs)
                args.Append(" -x \"" + s + "\"");

            // Followed by the list of keys to load
            foreach (string s in keys)
                args.Append(" \"" + s + "\"");

            // Run the pageant process if we have any (new) keys or phrases to import,
            // otherwise, run it only if it already is not running to avoid its warning message
            if (args.Length>0 || !IsProcessRunning("pageant"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _pathPageant;
                startInfo.Arguments = args.ToString();

                _procPageant = Process.Start(startInfo);
            }
        }

        /// <summary>
        /// Returns full path name to PLINK used to set up GIT_SSH variable when running a command
        /// </summary>
        public string GetPlinkPath()
        {
            return _pathPlink;
        }

        /// <summary>
        /// Returns the list of passphrases in plaintext format
        /// </summary>
        public List<string> GetPassPhrases()
        {
            // Base-64 encoded strings, zero-delimited, are read from application settings
            List<string> pfs = Properties.Settings.Default.PuTTYPf.
                Split(("\0").ToCharArray(), 
                      StringSplitOptions.RemoveEmptyEntries).
                ToList();

            // Select each string and tramsform it from Base-64 to plaintext
            pfs = pfs.Select(
                    s => Encoding.ASCII.GetString(
                    Convert.FromBase64String(s))).
                    ToList();
            return pfs;
        }

        /// <summary>
        /// Saves the list of passphrases into application properties
        /// </summary>
        public void SetPassPhrases(List<string> pfs)
        {
            // Convert each passphrase into Base-64 encoded string
            pfs = pfs.Select(
                s => Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(s))).
                    ToList();

            Properties.Settings.Default.PuTTYPf = String.Join("\0", pfs);
        }

        /// <summary>
        /// Initializes SSH connection by running the PLINK using the specified
        /// connection parameters. This function blocks until the PLINK returns.
        /// </summary>
        public void ImportRemoteSshKey(ClassUrl.Url url)
        {
            // Build the arguments to the PLINK process: port number, user and the host
            // Use the default SSH values if the url did not provide them
            string args = " -P " + (url.Port > 0 ? url.Port.ToString() : "22") +
                          " -l " + (string.IsNullOrEmpty(url.User) ? "anonymous" : url.User) +
                          " " + url.Host;

            // Start the process silently and wait until it ends
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _pathPlink;
            startInfo.EnvironmentVariables.Add("PLINK_PROTOCOL", "ssh");
            startInfo.UseShellExecute = false;
//          startInfo.CreateNoWindow = true;
            startInfo.Arguments = args;

            Process procPlink = Process.Start(startInfo);

            procPlink.WaitForExit();
        }
    }
}
