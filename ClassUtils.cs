using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Git4Win
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
    }
}
