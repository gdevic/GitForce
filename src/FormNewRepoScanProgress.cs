using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Scan for the list of directories that potentially host Git repositories
    /// </summary>
    public partial class FormNewRepoScanProgress : Form
    {
        /// <summary>
        /// List of potential candidates to contain a git repository
        /// </summary>
        public readonly List<string> Gits = new List<string>();

        /// <summary>
        /// Known system directory names to skip during scanning
        /// </summary>
        private static readonly HashSet<string> SystemDirNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "lost+found"
        };

        private readonly string dir;
        private readonly bool deepScan;
        private bool enableScan;

        public FormNewRepoScanProgress(string directory, bool fDeepScan)
        {
            InitializeComponent();
            dir = directory;
            deepScan = fDeepScan;
        }

        /// <summary>
        /// Check if a directory should be skipped during scanning.
        /// Skips system directories, hidden directories (starting with .), and known system folder names.
        /// </summary>
        private bool ShouldSkipDirectory(string path)
        {
            try
            {
                string name = Path.GetFileName(path);

                // Skip directories starting with "." (hidden on Linux/macOS), except .git
                if (name.StartsWith(".") && name != ".git")
                    return true;

                // Skip known system directory names
                if (SystemDirNames.Contains(name))
                    return true;

                // Skip directories with System attribute (Windows system folders)
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if ((dirInfo.Attributes & FileAttributes.System) != 0)
                    return true;
            }
            catch
            {
                // If we can't check attributes, don't skip
            }
            return false;
        }

        /// <summary>
        /// Recursively search folders starting at the given directory and
        /// add all paths that end with .git to the list of potential candidates.
        /// Also detects submodules by parsing .gitmodules files.
        /// </summary>
        private void SearchGit(string dir)
        {
            // Silently ignore unreachable directories
            try
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    textDir.Text = d;
                    Application.DoEvents();
                    if (enableScan == false)
                        return;

                    if (d.EndsWith(Path.DirectorySeparatorChar + ".git"))
                    {
                        string repoPath = d.Substring(0, d.Length - 5);
                        Gits.Add(repoPath);
                        // Check for submodules in this repo
                        AddSubmodules(repoPath);
                        if (deepScan == false)
                            break;
                    }
                    else if (!ShouldSkipDirectory(d))
                        SearchGit(d);
                }
            }
            catch (Exception) {}
        }

        /// <summary>
        /// Parse .gitmodules file and add any submodule paths to the list
        /// </summary>
        private void AddSubmodules(string repoPath)
        {
            string gitmodulesPath = Path.Combine(repoPath, ".gitmodules");
            if (!File.Exists(gitmodulesPath))
                return;

            try
            {
                string[] lines = File.ReadAllLines(gitmodulesPath);
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith("path = ") || trimmed.StartsWith("path="))
                    {
                        int eqIndex = trimmed.IndexOf('=');
                        string submodulePath = trimmed.Substring(eqIndex + 1).Trim();
                        string fullPath = Path.Combine(repoPath, submodulePath);
                        // Only add initialized submodules (have a .git file with gitdir reference)
                        string gitFile = Path.Combine(fullPath, ".git");
                        if (File.Exists(gitFile) && !Gits.Contains(fullPath))
                            Gits.Add(fullPath);
                    }
                }
            }
            catch (Exception) {}
        }

        /// <summary>
        /// Start scanning at the time the form is first shown
        /// </summary>
        private void FormNewRepoScanProgressShown(object sender, EventArgs e)
        {
            Gits.Clear();
            enableScan = true;
            try
            {
                SearchGit(dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enableScan = false;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Stop scanning and exit the dialog
        /// </summary>
        private void BtStopClick(object sender, EventArgs e)
        {
            enableScan = false;
            Application.DoEvents();
            DialogResult = DialogResult.OK;
        }
    }
}
