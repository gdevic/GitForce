using System;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormPathToGit : Form
    {
        /// <summary>
        /// Default path is the user's program files
        /// </summary>
        public string PathToGit;

        public FormPathToGit(string programFiles, string suggestPathToGit)
        {
            InitializeComponent();

            textBoxPath.Text = programFiles;

            labelInfo.Text = "GitForce is a GUI front-end to the command line git. " +
            "That means you have to have git already installed. You can download git for Windows from the link below.";
        }

        /// <summary>
        /// Lets the user browse to a directory containing git.
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = fileDlg.FileName;
            }
        }

        /// <summary>
        /// Open the web browser with a link to Git for Windows
        /// </summary>
        private void GitLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClassUtils.OpenWebLink(linkLabel.Tag.ToString());
        }

        /// <summary>
        /// As the text in the git path is changing, check for the git executable name
        /// and enable or disable the OK button based on this check
        /// </summary>
        private void TextBoxPathTextChanged(object sender, EventArgs e)
        {
            PathToGit = textBoxPath.Text;
            btOK.Enabled = File.Exists(PathToGit) && PathToGit.EndsWith("git.exe");
        }
    }
}
