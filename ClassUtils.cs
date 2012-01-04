using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Contains various utility functions
    /// </summary>
    public static class ClassUtils
    {
        private static string _lastError = string.Empty;

        /// <summary>
        /// Generic last error string
        /// </summary>
        public static string LastError
        {
            get { return _lastError; }
            set { _lastError = value; if (IsLastError()) App.Log.Print(value); }
        }

        /// <summary>
        /// Helper function that returns True if there was an error (LastError)
        /// </summary>
        public static bool IsLastError()
        {
            return _lastError != String.Empty;
        }

        /// <summary>
        /// Helper function that clears last error status
        /// </summary>
        public static void ClearLastError()
        {
            _lastError = String.Empty;
        }

        /// <summary>
        /// Writes binary resource to a temporary file
        /// </summary>
        public static string WriteResourceToFile(string pathName, string fileName, byte[] buffer)
        {
            string path = Path.Combine(pathName, fileName);
            try
            {
                using (var sw = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
            }
            catch (Exception ex)
            {
                App.Log.Print(ex.Message);
            }
            return path;
        }

        /// <summary>
        /// Returns true if the app is running on Mono (Linux), false if it is Windows
        /// </summary>
        public static bool IsMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        /// <summary>
        /// Open a command prompt at the specific directory
        /// </summary>
        public static void CommandPromptHere(string where)
        {
            Directory.SetCurrentDirectory(where);
            try
            {
                App.PrintStatusMessage("Command prompt at " + where);
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = false;

                // Add all environment variables listed
                foreach (var envar in ClassExecute.GetEnvars())
                    proc.StartInfo.EnvironmentVariables.Add(envar.Key, envar.Value);

                // WAR: Opening a command window/terminal is platform-specific
                if (IsMono())
                {
                    // TODO: This may not work on a non-Ubuntu system?
                    proc.StartInfo.FileName = @"/usr/bin/gnome-terminal";
                    proc.StartInfo.Arguments = "--working-directory=" + where;
                }
                else
                {
                    proc.StartInfo.FileName = "cmd.exe";
                }
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Command Prompt Here error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        /// <summary>
        /// Open a file browser/Explorer at the specific directory, optionally selecting a file
        /// </summary>
        public static void ExplorerHere(string where, string selFile)
        {
            try
            {
                App.PrintStatusMessage("Opening a file browser at " + where);

                // WAR: Opening an "Explorer" is platform-specific
                if (IsMono())
                {
                    // TODO: Start a Linux (Ubuntu?) file explorer in a more flexible way
                    Process.Start(@"/usr/bin/nautilus", "--browser " + where);
                }
                else
                {
                    string path = selFile == string.Empty
                                      ? "/e,\"" + where + "\""
                                      : "/e, /select,\"" + selFile + "\"";
                    App.Log.Print(path);
                    Process.Start("explorer.exe", path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Explorer Here error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        /// <summary>
        /// Executes a shell command
        /// </summary>
        public static string ExecuteShellCommand(string cmd, string args)
        {
            string ret = string.Empty;
            try
            {
                App.PrintStatusMessage("Shell execute: " + cmd + " " + args);

                // WAR: Shell execute is platform-specific
                if (IsMono())
                {
                    ret = ClassExecute.Run(cmd, args);
                }
                else
                {
                    args = "/c " + cmd + " " + args;
                    ret = ClassExecute.Run("cmd.exe", args);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Shell Execute error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ret;
        }

        /// <summary>
        /// Returns a string to be used by CMD/SHELL as argument when executing a command line command
        /// </summary>
        public static string GetShellExecFlags()
        {
            return IsMono() ? "-c" : "/K";
        }

        /// <summary>
        /// Identical to NET4.0 IsNullOrWhiteSpace()
        /// </summary>
        public static bool IsNullOrWhiteSpace(string s)
        {
            if (s == null)
                return true;
            return s.Trim().Length == 0;
        }

        /// <summary>
        /// Remove given folder and all files and subfolders under it.
        /// If fPreserveGit is true, all folders that are named ".git" will be preserved (not removed)
        /// If fPreserveRootFolder is true, the first (root) folder will also be preserved
        /// Return false if the function could not remove all folders, true otherwise.
        /// </summary>
        public static bool DeleteFolder(DirectoryInfo dirInfo, bool fPreserveGit, bool fPreserveRootFolder)
        {
            ClearLastError();
            try
            {
                DeleteRecursiveFolder(dirInfo, fPreserveGit, fPreserveRootFolder);
            }
            catch (Exception ex)
            {
                _lastError = ex.Message;
            }
            return !IsLastError();
        }

        /// <summary>
        /// Delete a directory and all files and subdirectories under it.
        /// TODO: This particular case could probably be optimized: do we really need 2 booleans coming in
        /// </summary>
        private static void DeleteRecursiveFolder(DirectoryInfo dirInfo, bool fPreserveGit, bool fPreserveRootFolder)
        {
            foreach (var subDir in dirInfo.GetDirectories())
            {
                if (fPreserveGit == false || !subDir.Name.EndsWith(".git"))
                    DeleteRecursiveFolder(subDir, false, false);
            }

            foreach (var file in dirInfo.GetFiles())
                DeleteFile(file.FullName);

            if (fPreserveRootFolder == false)
            {
                try
                {
                    dirInfo.Delete();
                }
                catch(Exception ex)
                {
                    _lastError = ex.Message;
                }
            }
        }

        /// <summary>
        /// Deletes a file from the local file system.
        /// Returns true if delete succeeded, false otherwise, with the _lastError set.
        /// </summary>
        public static bool DeleteFile(string name)
        {
            ClearLastError();
            try
            {
                FileInfo file = new FileInfo(name) {Attributes = FileAttributes.Normal};
                file.Delete();
            }
            catch (Exception ex)
            {
                _lastError = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Handle double-clicking on a file.
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        public static void FileDoubleClick(string file)
        {
            // Perform the required action on double-click
            string option = Properties.Settings.Default.DoubleClick;
            string program = Properties.Settings.Default.DoubleClickProgram;

            try
            {
                if (option == "1")
                    Process.Start(file);
                if (option == "2")
                    Process.Start(program, file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        /// <summary>
        /// Edit selected file using either the default editor (native OS file association,
        /// if the tag is null, or the editor program specified in the tag field.
        /// This is a handler for the context menu, edit tool bar button and also
        /// revision history view menus.
        /// </summary>
        public static void FileOpenFromMenu(object sender, string file)
        {
            try
            {
                if (sender is ToolStripMenuItem)
                {
                    object opt = (sender as ToolStripMenuItem).Tag;
                    if (opt != null)
                    {
                        Process.Start(opt.ToString(), file);
                        return;
                    }
                }
                Process.Start(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
