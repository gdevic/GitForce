using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormDiffMissing : Form
    {
        public FormDiffMissing()
        {
            InitializeComponent();

            if(!ClassUtils.IsMono())
            {
                // Disabling the auto-download of KDiff3 for now until I figure out
                // how to pick up a file from that SF site...
                //
                // labelInfo.Text += "If you prefer, I can download and install KDiff3 for you.";
                // btInstall.Visible = true;
            }
        }

        /// <summary>
        /// User clicked on a link to a diff utility, open it up in a web browser.
        /// </summary>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClassUtils.OpenWebLink((sender as LinkLabel).Tag.ToString());
        }

        /// <summary>
        /// User clicked on a Install KDiff3 button to download and install KDiff3
        /// (Windows only)
        ///
        /// *** This button was removed ***
        /// I left this code in place should it be needed.
        /// The button was downloading this specific executable and installing it.
        /// However, github does not provide easy file access. There are ways around
        /// it and perhaps it is worth redoing it. One day.
        ///
        /// </summary>
        private void BtInstallClick(object sender, EventArgs e)
        {
            string installerFile = Path.GetTempFileName(); // Junk name so we can safely call 'Delete'
            try
            {
                FormDownload formDownload = new FormDownload("Download KDiff3",
                    @"https://sourceforge.net/projects/kdiff3/files/kdiff3/0.9.98",
                    @">(?<file>\S+.exe)", "/download/");

                // If the download succeeded, run the installer file
                if (formDownload.ShowDialog() == DialogResult.OK)
                {
                    // Run the installer application that we just downloaded and wait for it to finish
                    installerFile = formDownload.TargetFile;
                    Process procInstaller = Process.Start(installerFile);
                    procInstaller.WaitForExit();

                    if(procInstaller.ExitCode==0)
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
