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
    public partial class FormNewRepoStep2 : Form
    {
        /// <summary>
        /// Destination directory to create a new repo
        /// </summary>
        public string Destination = "";
        public string Extra = "";
        public bool IsBare;
        private bool CloneOperation;

        public FormNewRepoStep2()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormNewRepoStep2FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Show or hide project name edit box if we are doing a clone operation
        /// </summary>
        public void SetForCloneOperation(bool isClone)
        {
            textBoxProjectName.Enabled = CloneOperation = labelCloneOperation.Enabled = isClone;
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

                // Additional check for clone operation:
                //  If a project name is not present, the root needs to exist and be an empty directory
                //     (the check for 'exist' is already made in the step above)
                if (CloneOperation && textBoxProjectName.Text.Trim().Length == 0)
                {
                    btOK.Enabled &= (Directory.GetFiles(textBoxRepoPath.Text).Length == 0) &&
                                    (Directory.GetDirectories(textBoxRepoPath.Text).Length == 0);
                }
            }
            catch (Exception ex)
            {
                App.Log.Print(ex.Message);
                btOK.Enabled = false;
            }

            Destination = Path.Combine(textBoxRepoPath.Text, textBoxProjectName.Text.Trim());
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
    }
}
