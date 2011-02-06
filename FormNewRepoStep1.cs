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
        public string Type = "empty";
        public string Local = "";
        public ClassRemotes.Remote Remote;

        public FormNewRepoStep1()
        {
            InitializeComponent();
            // Set the default remote name
            Remote.Name = "origin";
            remoteDisplay.Set(Remote);
            remoteDisplay.AnyTextChanged += SomeTextChanged;
        }

        /// <summary>
        /// Browse for the local path to directory to clone
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if (folderDlg.ShowDialog() == DialogResult.OK)
                textBoxLocal.Text = folderDlg.SelectedPath;
        }

        /// <summary>
        /// User changed the radio button source for the repo
        /// </summary>
        private void RbSourceCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                textBoxLocal.ReadOnly = true;
                btNext.Enabled = btBrowse.Enabled = remoteDisplay.Enabled = false;
                remoteDisplay.Enable(false, false);

                switch (Type = rb.Tag.ToString())
                {
                    case "empty":
                        btNext.Enabled = true;
                        break;
                    case "local":
                        textBoxLocal.ReadOnly = false;
                        btBrowse.Enabled = true;
                        btNext.Enabled = Path.IsPathRooted(textBoxLocal.Text) && Directory.Exists(Path.Combine(textBoxLocal.Text, ".git"));
                        Local = textBoxLocal.Text;
                        break;
                    case "remote":
                        remoteDisplay.Enabled = true;
                        remoteDisplay.Enable(true, true);
                        btNext.Enabled = remoteDisplay.IsValid();
                        break;
                }
            }
        }

        /// <summary>
        /// Text in the local directory path changed. Validate the entry.
        /// </summary>
        private void TextBoxLocalTextChanged(object sender, EventArgs e)
        {
            btNext.Enabled = Path.IsPathRooted(textBoxLocal.Text) && Directory.Exists(Path.Combine(textBoxLocal.Text, ".git"));
            Local = textBoxLocal.Text;
        }

        /// <summary>
        /// Callback function called when text in the remote repo editing has changed.
        /// </summary>
        private void SomeTextChanged(bool valid)
        {
            btNext.Enabled = valid;
            Remote = remoteDisplay.Get();
        }
    }
}
