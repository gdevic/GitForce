using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Shows a changelist or stash with the ability to "walk" the chain.
    /// Returns "DialogResult.No" to load a previous change
    /// Returns "DialogResult.Yes" to load a next change
    /// Otherwise returns "DialogResult.Cancel"
    /// </summary>
    public partial class FormShowChangelist : Form
    {
        /// <summary>
        /// Constructor, set the same font as for commit text box.
        /// </summary>
        public FormShowChangelist()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            textChangelist.Font = Properties.Settings.Default.commitFont;
        }

        /// <summary>
        /// Given a sha string, loads that commit into the form.
        /// </summary>
        public void LoadChangelist(string sha)
        {
            string cmd = "show -t " + sha;
            string[] response = new[]{string.Empty};
            ExecResult result = App.Repos.Current.Run(cmd);
            if(result.Success())
                response = result.stdout.Split((Environment.NewLine).ToCharArray());

            // Note: Clear() should remote all text, but for some reason it does not
            textChangelist.Clear();
            textChangelist.Text = "";
            foreach (string s in response)
            {
                // If a line starts with 'commit', insert a link
                if (s.StartsWith("commit "))
                {
                    textChangelist.SelectedText = "Commit ";
                    textChangelist.InsertLink(s.Split(' ')[1]);
                    textChangelist.SelectedText = Environment.NewLine;
                }
                else
                    textChangelist.SelectedText = s + Environment.NewLine;
            }
            textChangelist.Select(0, 0);
        }

        /// <summary>
        /// User clicked on a SHA link within the rich text box
        /// </summary>
        private void TextChangelistLinkClicked(object sender, LinkClickedEventArgs e)
        {
            // TODO: Not used at the moment
            // string sha = sender as string;
        }

        /// <summary>
        /// Load next commit in the list
        /// </summary>
        private void BtNextClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        /// <summary>
        /// Load previous commit in the list
        /// </summary>
        private void BtPrevClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        /// <summary>
        /// On form closing, save the dialog location and size
        /// </summary>
        private void FormShowChangelistFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this); 
        }
    }
}
