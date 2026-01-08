using System;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormSubmoduleInfo : Form
    {
        private ClassSubmodules.Submodule submodule;

        public FormSubmoduleInfo()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Load submodule data into the form
        /// </summary>
        public void LoadSubmodule(ClassSubmodules.Submodule sm)
        {
            submodule = sm;

            // Populate text fields
            textName.Text = sm.Name ?? "";
            textPath.Text = sm.Path ?? "";
            textUrl.Text = sm.Url ?? "(unknown)";
            textSha.Text = sm.Sha ?? "(unknown)";

            // Build status text
            string statusText;
            switch (sm.StatusCode)
            {
                case '-': statusText = "Not initialized"; break;
                case '+': statusText = "Modified (different commit)"; break;
                case 'U': statusText = "Merge conflict"; break;
                default: statusText = "OK"; break;
            }
            if (!sm.IsInitialized)
                statusText += " (not initialized)";
            textStatus.Text = statusText;

            // Check if submodule is already tracked in GitForce
            UpdateAddToReposButton();
        }

        /// <summary>
        /// Check if submodule path is already tracked in GitForce repos list
        /// </summary>
        private bool IsAlreadyTracked()
        {
            if (string.IsNullOrEmpty(submodule.Path))
                return false;
            string normalizedPath = Path.GetFullPath(submodule.Path);
            foreach (var repo in App.Repos.Repos)
            {
                string repoPath = Path.GetFullPath(repo.Path);
                if (repoPath.Equals(normalizedPath, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Update the Add to Repos button state based on whether the submodule is already tracked
        /// </summary>
        private void UpdateAddToReposButton()
        {
            bool alreadyTracked = IsAlreadyTracked();
            btAddToRepos.Enabled = !alreadyTracked && submodule.IsInitialized;

            if (alreadyTracked)
                toolTip.SetToolTip(btAddToRepos, "This submodule is already tracked in GitForce");
            else if (!submodule.IsInitialized)
                toolTip.SetToolTip(btAddToRepos, "Submodule must be initialized first");
            else
                toolTip.SetToolTip(btAddToRepos, "Add this submodule as a tracked repository in GitForce");
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormSubmoduleInfoFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Copy URL to clipboard
        /// </summary>
        private void BtCopyUrlClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(submodule.Url))
                Clipboard.SetText(submodule.Url);
        }

        /// <summary>
        /// Copy path to clipboard
        /// </summary>
        private void BtCopyPathClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(submodule.Path))
                Clipboard.SetText(submodule.Path);
        }

        /// <summary>
        /// Copy SHA to clipboard
        /// </summary>
        private void BtCopyShaClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(submodule.Sha))
                Clipboard.SetText(submodule.Sha);
        }

        /// <summary>
        /// Add submodule to GitForce repos list
        /// </summary>
        private void BtAddToReposClick(object sender, EventArgs e)
        {
            try
            {
                App.Repos.Add(submodule.Path);
                App.DoRefresh();
                UpdateAddToReposButton();
                MessageBox.Show("Submodule added to repository list.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error adding repository",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
