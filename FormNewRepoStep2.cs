using System;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormNewRepoStep2 : Form
    {
        /// <summary>
        /// Destination directory to create a new repo
        /// </summary>
        public string Destination
        {
            get { return ClassUtils.GetCombinedPath(textBoxRepoPath.Text.Trim(), textBoxProjectName.Text.Trim()); }
            set { textBoxRepoPath.Text = value; }
        }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName
        {
            get { return textBoxProjectName.Text.Trim(); }
            set { textBoxProjectName.Text = value; }
        }

        public string InitBranchName
        {
            get { return textBoxInitBranchName.Text.Trim(); }
            set { textBoxInitBranchName.Text = value; }
        }

        /// <summary>
        /// Optional extra parameters which can be set by the user
        /// </summary>
        public string Extra
        {
            get { return textBoxExtraArgs.Text.Trim(); }
        }

        public bool IsBare;
        public bool CheckTargetDirEmpty;

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
        /// Browse for the final path to the directory to init
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            folder.Description = @"Select a folder to host the new repository:";
            if (folder.ShowDialog() == DialogResult.OK)
                textBoxRepoPath.Text = folder.SelectedPath;
        }

        /// <summary>
        /// Text changed in the destination path, validate it.
        /// </summary>
        private void TextBoxRepoPathTextChanged(object sender, EventArgs e)
        {
            // Target folder needs to be a valid directory, with or without files in it
            ClassUtils.DirStatType type = ClassUtils.DirStat(textBoxRepoPath.Text);
            btOK.Enabled = type == ClassUtils.DirStatType.Empty || type == ClassUtils.DirStatType.Nongit;
            // Additional checks for clone operations (where CheckTargetDirEmpty is true)
            if (CheckTargetDirEmpty)
            {
                // If the project name is specified, that complete path should not exist
                if (textBoxProjectName.Text.Trim().Length > 0)
                    btOK.Enabled &= ClassUtils.DirStat(Destination) == ClassUtils.DirStatType.Invalid;
            }
        }

        /// <summary>
        /// Copy the bare checked state into a public variable that is exposed
        /// </summary>
        private void CheckBoxBareCheckedChanged(object sender, EventArgs e)
        {
            IsBare = checkBoxBare.Checked;
        }
    }
}
