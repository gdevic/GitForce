using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
{
    public partial class FormShowChangelist : Form
    {
        private Point _location;
        private Size _size;

        /// <summary>
        /// Constructor, set the same font as for commit text box.
        /// </summary>
        public FormShowChangelist()
        {
            InitializeComponent();

            textChangelist.Font = Properties.Settings.Default.commitFont;
        }

        /// <summary>
        /// Given a sha string, loads that commit into the form.
        /// </summary>
        public void LoadChangelist(string sha)
        {
            string cmd = "show -t " + sha;
            string[] response = App.Repos.Current.Run(cmd).Split(("\n").ToCharArray());

            textChangelist.Clear();
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
            string sha = sender as string;
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
        /// On form closing, save the dialog location and size since this
        /// form can be reopened
        /// </summary>
        private void FormShowChangelistFormClosing(object sender, FormClosingEventArgs e)
        {
            _location = Location;
            _size = Size;
        }

        /// <summary>
        /// Restore the dialog location and size if this is an iterative invocation
        /// and the size has actually been stored before
        /// </summary>
        private void FormShowChangelistActivated(object sender, EventArgs e)
        {
            if (_size.Width != 0)
            {
                Location = _location;
                Size = _size;
            }
        }
    }
}
