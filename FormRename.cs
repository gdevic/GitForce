using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private readonly List<string> inFiles = new List<string>();

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

            // Load the original list of files into the text box to show what will be renamed
            // Only load files, not directories
            foreach (string file in files.Where(file => !file.EndsWith(Convert.ToString(Path.DirectorySeparatorChar))))
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
                int i;
                bool fDone = false;
                // Iteratively find the common path prefix
                for (i = 0; !fDone; i++)
                {
                    char c = inFiles[0][i];
                    foreach (string file in inFiles)
                        if (file.Length == i || file[i] != c)
                            fDone = true;
                }
                i = inFiles[0].Substring(0, i).LastIndexOf('/');
                if (i > 0)
                {
                    multiFileCommonPath = inFiles[0].Substring(0, i);
                    textNewName.Text = multiFileCommonPath + @"/...";
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
        /// Returns a list of git commands to run for a rename.
        /// Call after the control returns with OK.
        /// </summary>
        public List<string> GetGitCmds()
        {
            List<string> cmds = new List<string>();
            if (inFiles.Count == 1)
            {
                cmds.Add("mv " + inFiles[0] + " " + textNewName.Text);
            }
            else
            {
                // Remove trailing "..." from "/..."
                // We know it's there since OK button would not be enabled without it
                string path = textNewName.Text;
                path = path.Substring(0, path.Length - 4);

                cmds.AddRange(from line in inFiles
                              select "mv " + line + " " + path + "/" + line);
            }
            return cmds;
        }

        /// <summary>
        /// Returns true if the new file path specification is valid
        /// </summary>
        private bool IsValid()
        {
            string path = Path.Combine(repo.Root, textNewName.Text);

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
                // Check valid path
                // Remove trailing spec "/..."
                if (path.EndsWith(@"/..."))
                {
                    path = path.Substring(0, path.Length - 4);

                    if (path.EndsWith(@"/"))
                        path = path.Substring(0, path.Length - 1);

                    if (multiFileCommonPath != path)
                    {
                        try
                        {
                            new DirectoryInfo(path);
                            return true;
                        }
                        catch { }
                    }
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
