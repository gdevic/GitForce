using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class FormPathToGit : Form
    {
        /// <summary>
        /// Default path is the user's program files
        /// </summary>
        public string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));

        public FormPathToGit()
        {
            InitializeComponent();

            textBoxPath.Text = path;
            labelInfo.Text = "git4win is a GUI front-end to the command line git. " +
            "That means you have to have git already installed. You can download git for Windows from the link below.";
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = fileDlg.FileName;
            }
        }

        /// <summary>
        /// Open the web browser with a link to msysgit
        /// </summary>
        private void gitLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        /// <summary>
        /// As the text in the git path is changing, check for the git executable name
        /// and enable or disable the OK button based on this check
        /// </summary>
        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            path = textBoxPath.Text;
            btOK.Enabled = File.Exists(path) && path.EndsWith("git.exe");
        }
    }
}
