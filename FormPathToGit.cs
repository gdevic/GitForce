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

        private string gitPath;

        public FormPathToGit(string programFiles, string suggestPathToGit)
        {
            InitializeComponent();

            textBoxPath.Text = programFiles;
            gitPath = suggestPathToGit;

            labelInfo.Text = "GitForce is a GUI front-end to the command line git. " +
            "That means you have to have git already installed. You can download git for Windows from the link below " +
            "(msysgit version is recomended), or you can click on the 'Install' button and I will do it for you.";
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
        /// Open the web browser with a link to msysgit
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

        /// <summary>
        /// User clicked on the Install button.. We need to find the latest msysgit build,
        /// download it and run it
        /// </summary>
        private void BtInstallClick(object sender, EventArgs e)
        {
            string installerFile = Path.GetTempFileName(); // Junk name so we can safely call 'Delete'
            try
            {
                // msysgit is hosted at https://github.com/msysgit/msysgit/releases
                // and the files can be downloaded at the subfolder 'download':

                FormDownload msysgit = new FormDownload("Download msysgit",
                    @"https://github.com/msysgit/msysgit/releases",
                    @"(?<file>Git-[1-2]+.[0-9]+.[0-9]+-\S+.exe)", "/download/");

                // If the download succeeded, run the installer file
                if(msysgit.ShowDialog()==DialogResult.OK)
                {
                    installerFile = msysgit.TargetFile;
                    ExecResult ret = Exec.Run(installerFile, String.Empty);
                    if(ret.retcode==0)
                    {
                        // Check if the git executable is at the expected location now
                        if (File.Exists(gitPath))
                            PathToGit = textBoxPath.Text = gitPath;
                    }
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                // Make sure we don't leave temporary files around
                File.Delete(installerFile);
            }
        }
    }
}
