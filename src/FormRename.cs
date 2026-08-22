using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Dialog to rename or move file or a set of files
    /// </summary>
    public partial class FormRename : Form
    {
        private ClassRepo repo;
        private string multiFileCommonPath;

        /// <summary>
        /// Trailing specification of a multi-file new name: a separator followed by "..."
        /// Paths handed to this dialog use the native separator (ClassStatus rewrites them),
        /// so the proposed name has to use it as well or nothing will ever match.
        /// </summary>
        private static readonly string PathSpecSuffix = Path.DirectorySeparatorChar + "...";

        private readonly List<string> inFiles = new List<string>();

        /// <summary>
        /// New name as typed by the user, with any separator rewritten to the native one
        /// so that a path typed with the other slash is still recognized. On Mono both
        /// separator characters are '/' and this is a no-op.
        /// </summary>
        private string NewName
        {
            get { return textNewName.Text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar); }
        }

        public FormRename()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormRenameFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Loads a file or a set of files and prepares the dialog controls
        /// Two different modes of opeation: single file or multiple files
        /// </summary>
        public bool LoadFiles(ClassRepo targetRepo, string[] files)
        {
            repo = targetRepo;

            // Load the original list of files into the text box to show what will be renamed.
            // Only load files, not directories: a folder selected together with its contents
            // arrives here with its trailing separator already trimmed off by Selection, so
            // it has to be recognized by asking the file system rather than by its name.
            inFiles.Clear();
            foreach (string file in files.Where(file => !Directory.Exists(Path.Combine(targetRepo.Path, file))))
                inFiles.Add(file);

            if (inFiles.Count == 0)
                return false;

            // Show files as relative to the repo root
            textOriginalNames.Clear();
            foreach (string file in inFiles)
                textOriginalNames.Text += @"//" + file + Environment.NewLine;

            // Set the New Name(s) accordingly
            if (inFiles.Count == 1)
            {
                // Single file - initial proposed new name is the same
                textNewName.Text = inFiles[0];
            }
            else
            {
                // Multiple files - proposed new name (filespec) is a common directory path
                // Iteratively find the common path prefix. The scan is bound by the shortest
                // name so that a path which is a prefix of another one does not run off the end.
                int max = inFiles.Min(file => file.Length);
                int i = 0;
                while (i < max && inFiles.All(file => file[i] == inFiles[0][i]))
                    i++;
                i = inFiles[0].Substring(0, i).LastIndexOf(Path.DirectorySeparatorChar);
                if (i > 0)
                {
                    multiFileCommonPath = inFiles[0].Substring(0, i);
                    textNewName.Text = multiFileCommonPath + PathSpecSuffix;
                }
            }

            // Set the changelist options
            comboChangelist.Items.Clear();
            foreach (ClassCommit bundle in repo.Commits.Bundle)
                comboChangelist.Items.Add(bundle);
            comboChangelist.Items.Add("New");
            comboChangelist.SelectedIndex = 0;

            return true;
        }

        /// <summary>
        /// Returns the destination of every file being moved, relative to the repo root and
        /// in the same order as the source files
        /// </summary>
        private List<string> GetTargets()
        {
            // A trailing separator has to go before the name is quoted: it would escape the
            // closing quote, and git would receive an argument ending in a quote character
            string path = NewName.TrimEnd(Path.DirectorySeparatorChar);

            if (inFiles.Count == 1)
                return new List<string> { path };

            // Remove the trailing "..." of the path specification. The separator in front of
            // it went with the trim above, so only the three dots are left to take off.
            path = path.Substring(0, path.Length - 3).TrimEnd(Path.DirectorySeparatorChar);

            // Each file keeps whatever followed the common path and moves under the new one
            int common = multiFileCommonPath == null ? 0 : multiFileCommonPath.Length;
            return (from line in inFiles
                    select path + (common == 0 ? Path.DirectorySeparatorChar + line : line.Substring(common))).ToList();
        }

        /// <summary>
        /// Returns a list of git commands to run for a rename.
        /// Call after the control returns with OK.
        /// </summary>
        public List<string> GetGitCmds()
        {
            List<string> targets = GetTargets();
            return inFiles.Select((file, i) => "mv \"" + file + "\" \"" + targets[i] + "\"").ToList();
        }

        /// <summary>
        /// Creates the directories this move needs. git mv does not create a missing
        /// destination directory, it fails the whole command instead, so without this every
        /// move into a new folder comes back as "No such file or directory".
        /// Returns false, having reported why, if a directory could not be created.
        /// </summary>
        public bool CreateTargetDirs()
        {
            try
            {
                foreach (string target in GetTargets())
                {
                    string dir = Path.GetDirectoryName(Path.Combine(repo.Path, target));
                    if (!string.IsNullOrEmpty(dir))
                        Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                App.PrintStatusMessage("Unable to create the destination directory: " + ex.Message, MessageType.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the new file path specification is valid
        /// </summary>
        private bool IsValid()
        {
            // This runs on every keystroke, and Path.Combine throws outright on a character
            // that is not legal in a path, so an invalid name is simply not a valid one
            string path;
            try
            {
                path = Path.Combine(repo.Path, NewName);
            }
            catch
            {
                return false;
            }

            // With a single file, check that the new file name is writable
            // With multiple files, check that the new path is accessible
            if (inFiles.Count == 1)
            {
                // Check file valid path
                try
                {
                    new FileInfo(path);
                    if (!File.Exists(path))
                        return true;
                }
                catch { }
            }
            else
            {
                // The specification has to be on the name as typed. Testing the combined
                // absolute path instead would accept a bare "..." , since the repo root
                // supplies the separator in front of it, and leave nothing to move into.
                if (!NewName.EndsWith(PathSpecSuffix))
                    return false;

                path = path.Substring(0, path.Length - PathSpecSuffix.Length).TrimEnd(Path.DirectorySeparatorChar);

                // Keep OK disabled while the target is still the original common path.
                // Both sides have to be absolute here since path came through Path.Combine,
                // and on Windows they have to compare without regard to case, or the
                // untouched default reads as a change and turns into a case-only rename.
                string original = multiFileCommonPath == null
                                      ? null
                                      : Path.Combine(repo.Path, multiFileCommonPath);
                StringComparison how = ClassUtils.IsMono() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                if (original == null || !string.Equals(original, path, how))
                {
                    try
                    {
                        new DirectoryInfo(path);
                        return true;
                    }
                    catch { }
                }
            }
            return false;
        }

        /// <summary>
        /// Callback when a text in the New Name(s) input field changed.
        /// Check the validity of the path and enable OK button accordingly.
        /// </summary>
        private void TextNewNameTextChanged(object sender, EventArgs e)
        {
            btOK.Enabled = IsValid();
        }
    }
}
