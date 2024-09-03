using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Manage PuTTY, PLink and PuttyGen utilities.
    /// This class should be instantiated on Windows OS only.
    /// </summary>
    public class ClassPutty
    {
        private readonly string pathPageant;
        private readonly string pathPlink;
        private readonly string pathPuttyGen;
        private Process procPageant;

        /// <summary>
        /// Constructor class function, create executables in the temp space
        /// </summary>
        public ClassPutty()
        {
            string pathPageantLong = ClassUtils.WriteResourceToFile(Path.GetTempPath(), "pageant.exe", Properties.Resources.pageant);
            string pathPlinkLong = ClassUtils.WriteResourceToFile(Path.GetTempPath(), "plink.exe", Properties.Resources.plink);
            string pathPuttyGenLong = ClassUtils.WriteResourceToFile(Path.GetTempPath(), "puttygen.exe", Properties.Resources.puttygen);

            pathPageant = ClassUtils.GetShortPathName(pathPageantLong);
            pathPlink = ClassUtils.GetShortPathName(pathPlinkLong);
            pathPuttyGen = ClassUtils.GetShortPathName(pathPuttyGenLong);

            ClassUtils.AddEnvar("PLINK_PROTOCOL", "ssh");
            ClassUtils.AddEnvar("GIT_SSH", pathPlink);

            // Run the daemon process, update keys
            RunPageantUpdateKeys();
        }

        /// <summary>
        /// Destructor for the class make sure the executable resources are properly disposed of
        /// </summary>
        ~ClassPutty()
        {
            // No real harm done if we fail to remove temp files. The next time GitForce is
            // run we will reuse the same files, so the temp folder will not grow.
            try
            {
                // Dont attempt to stop/remove Pageant if the user wanted to leave it running
                if (Properties.Settings.Default.leavePageant == false)
                {
                    if (procPageant != null)
                    {
                        if (!procPageant.HasExited)
                        {
                            // Send a notification to Pageant to close
                            IntPtr winHandle = NativeMethods.FindWindow("PageantSysTray", null);
                            NativeMethods.SendMessage(winHandle, NativeMethods.WM_COMMAND, NativeMethods.WM_CLOSE, 0);
                        }
                        //File.Delete(_pathPageant);
                    }
                }
                // Note: We leave these files in to allow secondary application instances to co-exist
                //File.Delete(_pathPlink);
                //File.Delete(_pathPuttyGen);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Run the PuTTYgen process and wait until it exits.
        /// </summary>
        public void RunPuTTYgen()
        {
            Process.Start(pathPuttyGen).WaitForExit();
        }

        /// <summary>
        /// Run plink program with the given arguments
        /// </summary>
        public void RunPLink(string args)
        {
            // Start a console process
            Process proc = new Process();
            proc.StartInfo.FileName = ClassUtils.GetShellExecCmd();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = false;

            // We need to keep the CMD/SHELL window open, so start the process using
            // the CMD/SHELL as the root process and pass it our command to execute
            proc.StartInfo.Arguments = string.Format("{0} \"{1}\" {2}",
                ClassUtils.GetShellExecFlags(), pathPlink, args);

            App.PrintLogMessage(proc.StartInfo.Arguments, MessageType.Command);

            proc.Start();
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
            // or if the pageant process is not running
            bool isPageantRunning = Process.GetProcesses().Any(proc => proc.ProcessName == "pageant");

            if (args.Length > 0 || isPageantRunning==false)
            {
                procPageant = new Process();
                procPageant.StartInfo.FileName = pathPageant;
                procPageant.StartInfo.Arguments = args.ToString();

                // TODO: Handle unsuccessful process start
                procPageant.Start();
            }
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

            Properties.Settings.Default.PuTTYPf = String.Join("\0", pfs.ToArray());
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

            // plink does everything through its stderr stream
            ExecResult result = Exec.Run(pathPlink, args);
            App.PrintLogMessage(result.stderr, MessageType.Error);
        }
    }
}
