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
    public partial class FormNewRepoStep1 : Form
    {
        /// <summary>
        /// Describes the source for the new repo: "empty", "local" or "remote"
        /// </summary>
        public string type = "empty";
        public string local = "";
        public ClassRemotes.Remote remote = new ClassRemotes.Remote();

        public FormNewRepoStep1()
        {
            InitializeComponent();
            // Set the default remote name
            remote.name = "origin";
            remoteDisplay.Set(remote);
            remoteDisplay.AnyTextChanged += SomeTextChanged;
        }

        /// <summary>
        /// Browse for the local path to directory to clone
        /// </summary>
        private void btBrowse_Click(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() == DialogResult.OK)
                textBoxLocal.Text = folderDlg.SelectedPath;
        }

        /// <summary>
        /// User changed the radio button source for the repo
        /// </summary>
        private void rbSource_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked == true)
            {
                textBoxLocal.ReadOnly = true;
                btNext.Enabled = btBrowse.Enabled = remoteDisplay.Enabled = false;
                remoteDisplay.Enable(false, false);

                switch (type = rb.Tag.ToString())
                {
                    case "empty":
                        btNext.Enabled = true;
                        break;
                    case "local":
                        textBoxLocal.ReadOnly = false;
                        btBrowse.Enabled = true;
                        btNext.Enabled = Path.IsPathRooted(textBoxLocal.Text) && Directory.Exists(Path.Combine(textBoxLocal.Text, ".git"));
                        local = textBoxLocal.Text;
                        break;
                    case "remote":
                        remoteDisplay.Enabled = true;
                        remoteDisplay.Enable(true, true);
                        btNext.Enabled = remoteDisplay.isValid();
                        break;
                }
            }
        }

        /// <summary>
        /// Text in the local directory path changed. Validate the entry.
        /// </summary>
        private void textBoxLocal_TextChanged(object sender, EventArgs e)
        {
            btNext.Enabled = Path.IsPathRooted(textBoxLocal.Text) && Directory.Exists(Path.Combine(textBoxLocal.Text, ".git"));
            local = textBoxLocal.Text;
        }

        /// <summary>
        /// Callback function called when text in the remote repo editing has changed.
        /// </summary>
        private void SomeTextChanged(bool valid)
        {
            btNext.Enabled = valid;
            remote = remoteDisplay.Get();
        }
    }
}
