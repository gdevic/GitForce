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
        private string currentSha;
        private readonly string CR;

        /// <summary>
        /// Constructor, set the same font as for commit text box.
        /// </summary>
        public FormShowChangelist()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            CR = Environment.NewLine;

            textChangelist.Font = Properties.Settings.Default.commitFont;
            comboShow.SelectedIndex = Properties.Settings.Default.ShowFormatIndex;
        }

        /// <summary>
        /// Given a sha string, loads that commit into the form.
        /// </summary>
        public void LoadChangelist(string sha)
        {
            currentSha = sha;

            // Issuing "show" command can take _very_ long time with a commit full of files
            // Run a much faster 'whatchanged' command first to get the list of files
            string cmd = "whatchanged " + currentSha + " -n 1 --format=medium";
            ExecResult result = App.Repos.Current.Run(cmd);
            string[] response = new[] { string.Empty };
            if (result.Success())
                response = result.stdout.Split(new[] { CR }, StringSplitOptions.None);

            // Go over the resulting list and add to our text box
            textChangelist.Text = "";       // Clear the rich text box
            bool reachedFiles = false;      // Flag helping us to write helpful section tag
            int numFiles = 0;               // Number of files counted

            foreach (string s in response)
            {
                // If a line starts with ":" parse it as file name indicator. Example:
                // :100644 000000 ed81075... 0000000... D  Build/Help.SED
                if (s.StartsWith(":"))
                {
                    // Print the "Files" section tag only once
                    if (reachedFiles == false)
                    {
                        textChangelist.AppendText(CR + "Files:" + CR + CR, Color.Red);
                        reachedFiles = true;
                    }

                    if (s.Length >= 39)    // Hard-coded value! Depends on the git output.
                    {
                        textChangelist.AppendText(s.Substring(37, 2));
                        textChangelist.InsertLink(s.Substring(39) + CR);
                    }
                    else
                        textChangelist.AppendText(s + CR);

                    numFiles++;
                }
                else
                    // If a line starts with 'commit', insert a link
                    if (s.StartsWith("commit "))
                    {
                        textChangelist.AppendText("Commit ");
                        textChangelist.InsertLink(s.Split(' ')[1]);
                        textChangelist.AppendText(CR);
                    }
                    else
                        textChangelist.AppendText(s + CR);
            }

            // Now optionally run the detailed show command, but if the number of files is large,
            // ask the user to confirm for any more than, say, 30 files
            if (comboShow.SelectedIndex==0)
                return;

            if (numFiles > 30)
            {
                string q =
                    "The number of files changed in this commit is very large and it may take considerable time" +
                    " to display their detailed difference. Do you still want to proceed?\n\n(To skip this message in the future, select " +
                    "(none) in the details option)";

                if (MessageBox.Show(q, "Detailed difference of " + numFiles + " files", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
            }

            cmd = "show -t " + currentSha + " --format=" + comboShow.SelectedItem;
            result = App.Repos.Current.Run(cmd);
            if (result.Success())
                response = result.stdout.Split(new[] { CR }, StringSplitOptions.None);

            textChangelist.AppendText(CR + "Details" + CR + CR, Color.Red);

            foreach (string s in response)
            {
                textChangelist.SelectedText = s + CR;
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
        /// User changed the show mode (format)
        /// </summary>
        private void ComboShowSelectedIndexChanged(object sender, EventArgs e)
        {
            if( Properties.Settings.Default.ShowFormatIndex != comboShow.SelectedIndex)
            {
                Properties.Settings.Default.ShowFormatIndex = comboShow.SelectedIndex;
                LoadChangelist(currentSha);
            }
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
