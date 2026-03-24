using System;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormNewRepoStep1 : Form
    {
        /// <summary>
        /// Describes source for a new repo: "empty", "local" or "remote"
        /// </summary>
        public string Type { get { return type; }
            set { type = value;
            if (type.Equals("empty")) rbEmpty.Checked = true;
            if (type.Equals("local")) rbLocal.Checked = true;
            if (type.Equals("remote")) rbRemote.Checked = true;
        }}
        private string type;

        /// <summary>
        /// Path to a local git repo
        /// </summary>
        public string Local {
            get { return textBoxLocal.Text.Trim(); }
            set { textBoxLocal.Text = value; }
        }

        /// <summary>
        /// ClassRemote structure of a new repo
        /// </summary>
        public ClassRemotes.Remote Remote;

        /// <summary>
        /// Call this method to properly set the remote structure that is going to be used by this dialog
        /// </summary>
        public void SetRemote(ClassRemotes.Remote remote)
        {
            Remote = remote;
            remoteEdit.Set(Remote);
        }

        public FormNewRepoStep1()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            Type = "empty";
            Local = String.Empty;

            // Set the default remote name
            Remote.Name = "origin";
            Remote.PushCmd = "";
            remoteEdit.Set(Remote);
            remoteEdit.AnyTextChanged += SomeTextChanged;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormNewRepoStep1FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Browse for the local path to directory to clone
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            folder.Description = @"Select a folder containing the root of a repository to clone:";
            if (folder.ShowDialog() == DialogResult.OK)
                Local = folder.SelectedPath;
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
                btNext.Enabled = btBrowse.Enabled = remoteEdit.Enabled = false;
                remoteEdit.Enable(false, false);

                switch (Type = rb.Tag.ToString())
                {
                    case "empty":
                        btNext.Enabled = true;
                        break;
                    case "local":
                        textBoxLocal.ReadOnly = false;
                        btBrowse.Enabled = true;
                        btNext.Enabled = ClassUtils.DirStat(Local) == ClassUtils.DirStatType.Git;
                        break;
                    case "remote":
                        remoteEdit.Enabled = true;
                        remoteEdit.Enable(true, true);
                        btNext.Enabled = remoteEdit.IsValid();
                        break;
                }
            }
        }

        /// <summary>
        /// Text in the local directory path changed. Validate the entry.
        /// </summary>
        private void TextBoxLocalTextChanged(object sender, EventArgs e)
        {
            btNext.Enabled = ClassUtils.DirStat(Local) == ClassUtils.DirStatType.Git;
        }

        /// <summary>
        /// Callback function called when text in the remote repo editing has changed.
        /// </summary>
        private void SomeTextChanged(bool valid)
        {
            btNext.Enabled = valid;
            Remote = remoteEdit.Get();
        }
    }
}
