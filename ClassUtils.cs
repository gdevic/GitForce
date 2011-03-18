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
                // WAR: Opening a command window/terminal is platform-specific
                if (ClassUtils.IsMono())
                {
                    // TODO: Start a terminal on Unix in a more flexible way
                    Process.Start(@"/usr/bin/gnome-terminal", "--working-directory=" + where);
                }
                else
                    Process.Start("cmd.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
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
            {
                file.Attributes = FileAttributes.Normal;
                try
                {
                    file.Delete();
                }
                catch(Exception ex)
                {
                    _lastError = ex.Message;
                }
            }

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
    }
}
