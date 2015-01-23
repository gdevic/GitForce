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
        /// Recursively search folders starting at the given directory and
        /// add all paths that end with .git to the list of potential candidates
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
                    if(enableScan==false)
                        return;

                    if (d.EndsWith(Path.DirectorySeparatorChar + ".git"))
                    {
                        Gits.Add(d.Substring(0, d.Length - 5));
                        if (deepScan == false)
                            break;
                    }
                    else
                        SearchGit(d);
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
