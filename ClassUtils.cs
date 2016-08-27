using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Contains various utility functions
    /// </summary>
    public static class ClassUtils
    {
        /// <summary>
        /// Set of environment variables used by the execution environment
        /// </summary>
        private static readonly Dictionary<string, string> Env = new Dictionary<string, string>();

        /// <summary>
        /// Adds an environment variable for the Run method
        /// </summary>
        public static void AddEnvar(string name, string value)
        {
            if (Env.ContainsKey(name))
                Env[name] = value;
            else
                Env.Add(name, value);
        }

        /// <summary>
        /// Returns a set of environment variables registered for this execution environment
        /// </summary>
        public static Dictionary<string, string> GetEnvars()
        {
            return Env;
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
                App.PrintLogMessage(ex.Message, MessageType.Error);
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
            try
            {
                Directory.SetCurrentDirectory(where);
                App.PrintStatusMessage("Command prompt at " + where, MessageType.General);
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = false;

                // Add all environment variables listed
                foreach (var envar in Env)
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
                App.PrintStatusMessage("Opening a file browser at " + where, MessageType.General);

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
                    App.PrintLogMessage(path, MessageType.Command);
                    Process.Start("explorer.exe", path);
                }
            }
            catch (Exception ex)
            {
                App.PrintLogMessage(ex.Message, MessageType.Error);
                MessageBox.Show(ex.Message, "Explorer Here error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Executes a shell command
        /// </summary>
        public static string ExecuteShellCommand(string cmd, string args)
        {
            App.PrintStatusMessage("Shell execute: " + cmd + " " + args, MessageType.Command);

            // WAR: Shell execute is platform-specific
            if (IsMono())
            {
                return Exec.Run(cmd, args).ToString();
            }
            else
            {
                args = "/c " + cmd + " " + args;
                return Exec.Run("cmd.exe", args).ToString();
            }
        }

        /// <summary>
        /// Returns CMD/SHELL executable name to execute a command line command
        /// </summary>
        public static string GetShellExecCmd()
        {
            return IsMono() ? "sh" : "cmd.exe";
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
        /// This function is meaningful only on Windows: Given a long file path name,
        /// return its short version. This is used mainly to avoid various problems with
        /// paths containing spaces and the inability of git to handle them.
        /// </summary>
        public static string GetShortPathName(string path)
        {
            if (IsMono())
                return path;
            var pathBuilder = new StringBuilder(1024);
            NativeMethods.GetShortPathName(path, pathBuilder, pathBuilder.Capacity);
            return pathBuilder.ToString();
        }

        /// <summary>
        /// Retruns a path to the home directory.
        /// </summary>
        public static string GetHomePath()
        {
            if (IsMono())
                return Environment.GetEnvironmentVariable("HOME");
            // On Windows, path to the user's home is a combination of a drive and a path
            string drive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            string path = Environment.GetEnvironmentVariable("HOMEPATH");
            // On Windows, the Path.Combine is fundamentally broken can can't be used
            return drive + path;
        }

        /// <summary>
        /// List of possible directory types returned by DirStat() method
        /// </summary>
        public enum DirStatType
        {
            Invalid,            // Specified path does not exist or is not a valid absolute directory path
            Empty,              // Specified path is a directory which contains no other sub-directories or files
            Git,                // Specified path is a root directory of a git repo
            Nongit              // Specified path is a general, non-git repo directory
        }

        /// <summary>
        /// Given a fully qualified path to a local directory, return the stat on that folder
        /// </summary>
        public static DirStatType DirStat(string path)
        {
            // Check that the path is valid and that it specifies an existing directory
            if (!Directory.Exists(path))
                return DirStatType.Invalid;
            // We don't allow relative paths, all paths need to be absolute or UNC
            if (!Path.IsPathRooted(path))       // Note: This call may throw exceptions but the preceeding call to
                return DirStatType.Invalid;     // Directory.Exists() will reject such invalid paths w/o exceptions
            // Second check if the directory is completely empty (no files of subdirectories within it)
            if ((Directory.GetFiles(path).Length == 0) && (Directory.GetDirectories(path).Length == 0))
                return DirStatType.Empty;
            // Lastly, check if there is a subdirectory ".git" with a representative file ("config") which
            // would make very likely that a given path is the root to a valid git folder structure
            string testFile = Path.Combine(path, Path.Combine(".git", "config"));
            return File.Exists(testFile) ? DirStatType.Git : DirStatType.Nongit;
        }

        /// <summary>
        /// Remove given folder and all files and subfolders under it.
        /// If fPreserveGit is true, all folders that are named ".git" will be preserved (not removed)
        /// If fPreserveRootFolder is true, the first (root) folder will also be preserved
        /// Return false if the function could not remove all folders, true otherwise.
        /// </summary>
        public static bool DeleteFolder(DirectoryInfo dirInfo, bool fPreserveGit, bool fPreserveRootFolder)
        {
            bool f = true;
            try
            {
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    if (fPreserveGit == false || !subDir.Name.EndsWith(".git"))
                        f &= DeleteFolder(subDir, false, false);
                }

                foreach (var file in dirInfo.GetFiles())
                    f &= DeleteFile(file.FullName);

                if (fPreserveRootFolder == false)
                    f &= DeleteFolder(dirInfo);
            }
            catch (Exception ex)
            {
                App.PrintLogMessage("Error deleting directory " + dirInfo.FullName + ": " + ex.Message, MessageType.Error);
            }
            return f;
        }

        /// <summary>
        /// Deletes a folder from the local file system.
        /// Returns true if delete succeeded, false otherwise.
        /// </summary>
        private static bool DeleteFolder(DirectoryInfo dirInfo)
        {
            try
            {
                dirInfo.Attributes = FileAttributes.Normal;
                dirInfo.Delete();
            }
            catch (Exception ex)
            {
                App.PrintLogMessage("Error deleting directory " + dirInfo.FullName + ": " + ex.Message, MessageType.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes a file from the local file system.
        /// Returns true if delete succeeded, false otherwise.
        /// </summary>
        public static bool DeleteFile(string name)
        {
            try
            {
                FileInfo file = new FileInfo(name) {Attributes = FileAttributes.Normal};
                file.Delete();
            }
            catch (Exception ex)
            {
                App.PrintLogMessage("Error deleting file " + name + ": " + ex.Message, MessageType.Error);
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Open an HTML link in an external web browser application.
        /// </summary>
        public static void OpenWebLink(string html)
        {
            try
            {
                Process.Start(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GitForce", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// Returns a copy of the input string, but with all non-ASCII characters stripped down.
        /// This function also removes ANSI escape codes from the string.
        /// </summary>
        public static string ToPlainAscii(string s)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsControl(s[i]) || s[i]=='\n')
                    str.Append(s[i]);
                else
                {   // Strip ANSI escape codes from the string
                    // http://ascii-table.com/ansi-escape-sequences.php
                    // Skip all non-characters (ANSI code terminates with a alpha character)
                    if (s[i] == 27 && i<s.Length-1 && s[i+1]=='[')
                        while (i < s.Length && !Char.IsLetter(s[i])) i++;
                    // Get rid of the terminating ANSI character
                    if (i<s.Length && Char.IsLetter(s[i]))
                        i++;
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Upgrades the application settings to a new version, if necessary
        /// http://stackoverflow.com/questions/10121044/app-loses-all-settings-when-app-update-is-installed
        /// </summary>
        public static void UpgradeApplicationSettingsIfNecessary()
        {
            // Application settings are stored in a subfolder named after the full #.#.#.# version number of the program.
            // This means that when a new version of the program is installed, the old settings will not be available.
            // Fortunately, there's a method called Upgrade() that you can call to upgrade the settings from the old to the new folder.
            // We control when to do this by having a boolean setting called 'NeedSettingsUpgrade' which is defaulted to true.
            // Therefore, the first time a new version of this program is run, it will have its default value of true.
            // This will cause the code below to call "Upgrade()" which copies the old settings to the new.
            // It then sets "NeedSettingsUpgrade" to false so the upgrade won't be done the next time.
            if (Properties.Settings.Default.NeedSettingsUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.NeedSettingsUpgrade = false;
            }
        }
    }
}