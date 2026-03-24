using System;
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
    }
}
