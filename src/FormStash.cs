using System;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Form implementing the git stash command
    /// </summary>
    public partial class FormStash : Form
    {
        public FormStash()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormStashFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// User clicked on the stash button to stash files
        /// </summary>
        private void BtStashClick(object sender, EventArgs e)
        {
            string cmd = String.Format("stash save {0} {1}",
                                       checkKeepIndex.Checked ? "--keep-index" : "",
                                       string.IsNullOrEmpty(textName.Text) ? "" : textName.Text);

            App.Repos.Current.RunCmd(cmd);
        }
    }
}
