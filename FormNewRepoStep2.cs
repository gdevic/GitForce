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
        public string destination = "";
        public string extra = "";
        public bool isBare = false;

        /// <summary>
        /// This variable can be set by the caller to...
        /// </summary>
        public bool enforceDirEmpty = false;

        public FormNewRepoStep2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Browse for the final path to the directory to init
        /// </summary>
        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() == DialogResult.OK)
                textBoxRepoPath.Text = folderDlg.SelectedPath;
        }

        /// <summary>
        /// Text changed in the destination path, validate it.
        /// </summary>
        private void textBoxRepoPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                btOK.Enabled = Path.IsPathRooted(textBoxRepoPath.Text) && Directory.Exists(textBoxRepoPath.Text);

                // Additional check for directory being empty
                if (enforceDirEmpty)
                    btOK.Enabled &= (Directory.GetFiles(textBoxRepoPath.Text).Length == 0) &&
                                    (Directory.GetDirectories(textBoxRepoPath.Text).Length == 0);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            destination = textBoxRepoPath.Text;
        }

        /// <summary>
        /// Copy the bare checked state into a public variable that is exposed
        /// </summary>
        private void checkBoxBare_CheckedChanged(object sender, EventArgs e)
        {
            isBare = checkBoxBare.Checked;
        }

        /// <summary>
        /// Copy the extra string into a public variable that is exposed
        /// </summary>
        private void textBoxExtraArgs_TextChanged(object sender, EventArgs e)
        {
            extra = textBoxExtraArgs.Text;
        }

        /// <summary>
        /// Clicking on the Pageant button starts the pageant process
        /// </summary>
        private void btPageant_Click(object sender, EventArgs e)
        {
            FormPuTTY formPuTTY = new FormPuTTY();
            formPuTTY.ShowDialog();
        }
    }
}
