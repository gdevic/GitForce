using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Scan for new repositories
    /// </summary>
    public partial class FormNewRepoScan : Form
    {
        public FormNewRepoScan()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            checkBoxDeepScan.Checked = Properties.Settings.Default.RepoDeepScan;
        }

        private void FormNewRepoScanFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Return a list of selected directories
        /// </summary>
        public List<string> GetList()
        {
            return listRepos.Items.Cast<object>().
                Where((t, i) => listRepos.GetItemCheckState(i) == CheckState.Checked).
                Select(t => t.ToString()).ToList();
        }

        /// <summary>
        /// Browse for a initial directory to scan from
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                textRoot.Text = folderDlg.SelectedPath;
            }
        }

        /// <summary>
        /// Start scanning
        /// </summary>
        private void BtScanClick(object sender, EventArgs e)
        {
            FormNewRepoScanProgress formScanProgress = new FormNewRepoScanProgress(textRoot.Text, checkBoxDeepScan.Checked);
            if (formScanProgress.ShowDialog() == DialogResult.OK)
            {
                // Add only unique values to the list, so we can run the scan on multiple
                // directories and add all scanned paths, even if they have common folders
                foreach (var path in formScanProgress.Gits.ToArray().
                    Where(path => !listRepos.Items.Contains(path)))
                        listRepos.Items.Add(path);
                btSelectAll.Enabled = btSelectNone.Enabled = listRepos.Items.Count > 0;
                BtSelectAllClick(null, null);
            }
        }

        /// <summary>
        /// Select all directories
        /// </summary>
        private void BtSelectAllClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listRepos.Items.Count; i++)
                listRepos.SetItemChecked(i, true);
        }

        /// <summary>
        /// Select no directory from the list
        /// </summary>
        private void BtSelectNoneClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listRepos.Items.Count; i++)
                listRepos.SetItemChecked(i, false);
        }

        /// <summary>
        /// On text in the root directory changed, verify the directory and enable Scan button
        /// </summary>
        private void TextRootTextChanged(object sender, EventArgs e)
        {
            btScan.Enabled = Path.IsPathRooted(textRoot.Text) && Directory.Exists(textRoot.Text);
        }

        /// <summary>
        /// Item in the list of paths is being checked, adjust button enables.
        /// </summary>
        private void ListReposItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                btAdd.Enabled = true;
        }
    }
}
