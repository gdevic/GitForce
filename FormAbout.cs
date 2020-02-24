﻿using System;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // Add the version number and the build date from the assembly info file
            labelVersion.Text = "Version " + ClassVersion.GetVersion();
            labelBuild.Text = ClassVersion.GetBuild();

            // If there is a new version available, show the label and a button
            labelNewVersionAvailable.Visible = btDownload.Visible = App.Version.NewVersionAvailable;

            textLic.Text =
                "THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR " +
                "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY," +
                "FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE " +
                "AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER " +
                "LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, " +
                "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN " +
                "THE SOFTWARE.";
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormAboutFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// User clicked on a link to the GPL license
        /// </summary>
        private void LinkGplClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClassHelp.Handler("GPL");
        }

        private void LinkBaltazarStudiosLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClassHelp.Handler("BaltazarStudios");
        }

        /// <summary>
        /// User clicked on the "Copy Email" button
        /// Copy the email address to clipboard.
        /// </summary>
        private void BtCopyEmailClick(object sender, EventArgs e)
        {
            Clipboard.SetText("BaltazarStudios@gmail.com");
        }

        /// <summary>
        /// User clicked a button to download a new version
        /// </summary>
        private void DownloadClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Download");
        }
    }
}
