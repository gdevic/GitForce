using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class FormNewRepoStep2 : Form
    {
        /// <summary>
        /// Destination directory to create a new repo
        /// </summary>
        public string Destination = "";
        public string Extra = "";
        public bool IsBare;

        /// <summary>
        /// This variable can be set by the caller to...
        /// </summary>
        public bool EnforceDirEmpty;

        public FormNewRepoStep2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Browse for the final path to the directory to init
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() == DialogResult.OK)
                textBoxRepoPath.Text = folderDlg.SelectedPath;
        }

        /// <summary>
        /// Text changed in the destination path, validate it.
        /// </summary>
        private void TextBoxRepoPathTextChanged(object sender, EventArgs e)
        {
            try
            {
                btOK.Enabled = Path.IsPathRooted(textBoxRepoPath.Text) && Directory.Exists(textBoxRepoPath.Text);

                // Additional check for directory being empty
                if (EnforceDirEmpty)
                    btOK.Enabled &= (Directory.GetFiles(textBoxRepoPath.Text).Length == 0) &&
                                    (Directory.GetDirectories(textBoxRepoPath.Text).Length == 0);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            Destination = textBoxRepoPath.Text;
        }

        /// <summary>
        /// Copy the bare checked state into a public variable that is exposed
        /// </summary>
        private void CheckBoxBareCheckedChanged(object sender, EventArgs e)
        {
            IsBare = checkBoxBare.Checked;
        }

        /// <summary>
        /// Copy the extra string into a public variable that is exposed
        /// </summary>
        private void TextBoxExtraArgsTextChanged(object sender, EventArgs e)
        {
            Extra = textBoxExtraArgs.Text;
        }

        /// <summary>
        /// Clicking on the Pageant button starts the pageant process
        /// </summary>
        private static void BtPageantClick(object sender, EventArgs e)
        {
            FormPuTTY formPuTTY = new FormPuTTY();
            formPuTTY.ShowDialog();
        }
    }
}
