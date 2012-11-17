using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
                labelInfo.Text += " If you click on the 'Install' button below, I will download and install KDiff3 for you.";
                btInstall.Visible = true;
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
        /// </summary>
        private void BtInstallClick(object sender, EventArgs e)
        {
            string installerFile = Path.GetTempFileName(); // Junk name so we can safely call 'Delete'
            try
            {
                FormDownload formDownload = new FormDownload("Download KDiff3",
                    @"https://github.com/gdevic/KDiff3/downloads/",
                    @">(?<file>\S+.exe)", true);

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
