using System;
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
        /// User clicked on a link label, open a web site
        /// </summary>
        private void LinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClassUtils.OpenWebLink((sender as LinkLabel).Tag.ToString());
        }

        /// <summary>
        /// User clicked on the "Copy Email" button
        /// Copy the email address to clipboard.
        /// </summary>
        private void btCopyEmail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("GitForce.Project@gmail.com");
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
