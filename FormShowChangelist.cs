using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly string cr;     // Just a convinient shortcut

        /// <summary>
        /// Constructor, set the same font as for commit text box.
        /// </summary>
        public FormShowChangelist()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
            cr = Environment.NewLine;

            textChangelist.Font = Properties.Settings.Default.commitFont;
            comboShow.SelectedIndex = Properties.Settings.Default.ShowFormatIndex;
        }

        /// <summary>
        /// On form closing, save the dialog location and size
        /// </summary>
        private void FormShowChangelistFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Describe (view) selected changelist and drive up/down back propagation through the host list view
        /// </summary>
        static public void DriveChangelistFromListViewEx(ref ListViewEx listRev)
        {
            // Get the SHA associated with the selected item on the log list
            ListView li = listRev;
            if (li.SelectedIndices.Count != 1)
                return;
            bool multiSel = li.MultiSelect;
            li.MultiSelect = false;
            int index = li.SelectedIndices[0];

            FormShowChangelist form = new FormShowChangelist();
            DialogResult dlg;

            do
            {
                li.Items[index].Selected = true;
                string sha = li.Items[index].Tag.ToString();
                form.LoadChangelist(sha);
                dlg = form.ShowDialog();

                // Using the "Yes" value to load a next commit
                if (dlg == DialogResult.Yes && index > 0)
                    index--;

                // Using the "No" value to load a previous commit
                if (dlg == DialogResult.No && index < li.Items.Count - 1)
                    index++;

            } while (dlg != DialogResult.Cancel);
            li.MultiSelect = multiSel;
        }

        /// <summary>
        /// Given a Sha string, loads that commit into the form.
        /// </summary>
        public void LoadChangelist(string sha)
        {
            Tag = sha;      // Store the SHA of a current commit in the Tag field of this form

            // Issuing "show" command can take _very_ long time with a commit full of files
            // Run a much faster 'whatchanged' command first to get the list of files
            string cmd = "whatchanged " + sha + " -n 1 --format=medium";
            ExecResult result = App.Repos.Current.Run(cmd);
            string[] response = new[] { string.Empty };
            if (result.Success())
                response = result.stdout.Split(new[] { cr }, StringSplitOptions.None);

            // Go over the resulting list and add to our text box
            textChangelist.Text = "";       // Clear the rich text box

            // Get the list of lines that describe individual files vs the rest (checkin comment)
            List<string> files = response.Where(s => s.StartsWith(":")).ToList();
            List<string> comment = response.Where(s => !s.StartsWith(":")).ToList();

            // ---------------- Print the comment section ----------------
            foreach (string s in comment)
                textChangelist.AppendText(s + cr);

            // ---------------- Print the files section ----------------
            textChangelist.AppendText(cr + "Files:" + cr + cr, Color.Red);

            foreach (string s in files)
            {
                // Parse the file name indicator following this template:
                // :100644 000000 ed81075... 0000000... D  Build/Help.SED
                // [attributes]   [prev]     [next]        [file]           (meaning)
                // 0       1      2          3          4  5                (chunk)

                // We strip tabs and '.' characters which are part of SHA keys in the line
                string[] chunk = s.Replace('.', ' ').Split(new[] { ' ', '\t', '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (s.Length >= 39)    // Hard-coded value! Depends on the git output.
                {
                    // In the tag of the link, we will send file name and related SHA keys
                    string tag = string.Format("{0}#{1}#{2}", s.Substring(39), chunk[2], chunk[3]);
                    textChangelist.AppendText(s.Substring(37, 2));
                    textChangelist.InsertLink(s.Substring(39), tag);
                    textChangelist.AppendText(cr);
                }
                else
                    textChangelist.AppendText(s + cr);
            }

            // Now optionally run the detailed show command, but if the number of files is large,
            // ask the user to confirm for any more than, say, 30 files
            if (comboShow.SelectedIndex==0)
                return;

            if (files.Count > 30)
            {
                string q =
                    "The number of files changed in this commit is very large and it may take considerable time" +
                    " to display their detailed difference. Do you still want to proceed?\n\n(To skip this message in the future, select " +
                    "(none) in the details option)";

                if (MessageBox.Show(q, "Detailed difference of " + files.Count + " files", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
            }

            cmd = "show -t " + sha + " --format=" + comboShow.SelectedItem;
            result = App.Repos.Current.Run(cmd);
            if (result.Success())
                response = result.stdout.Split(new[] { cr }, StringSplitOptions.None);

            textChangelist.AppendText(cr + "Details" + cr + cr, Color.Red);

            // Write out the complete response text containing all files' differences
            foreach (string s in response)
                textChangelist.SelectedText = s + cr;

            textChangelist.Select(0, 0);
        }

        /// <summary>
        /// User left-clicked on a file link within the rich text box
        /// </summary>
        private void TextChangelistLinkClicked(object sender, LinkClickedEventArgs e)
        {
            // The LinkText as sent contains the text + link separated by '#' character.
            // Use the link portion only to send to our handlers.
            string[] chunk = e.LinkText.Split('#');
            contextMenuFile.Tag = chunk[1];
            if (chunk[2] != "0000000")      // SHA of the previous revision of a file
            {
                menuItemDiffPrev.Enabled = true;
                menuItemDiffPrev.Tag = Tag + "^.." + Tag;
            }
            else
                menuItemDiffPrev.Enabled = false;

            if (chunk[3] != "0000000")       // SHA of the next revision of a file
            {
                menuItemDiffNext.Enabled = true;
                menuItemDiffNext.Tag = chunk[3];
            }
            else
                menuItemDiffNext.Enabled = false;

            // If the next revision exists, compare against a possible HEAD
            menuItemDiffHead.Enabled = menuItemDiffNext.Enabled;
            menuItemDiffHead.Tag = Tag + "..HEAD";

            contextMenuFile.Show(Cursor.Position);
        }

        /// <summary>
        /// User changed the show mode (format)
        /// 'This' Tag contains the SHA of a current commit
        /// </summary>
        private void ComboShowSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ShowFormatIndex != comboShow.SelectedIndex)
            {
                Properties.Settings.Default.ShowFormatIndex = comboShow.SelectedIndex;
                LoadChangelist(Tag.ToString());
            }
        }

        /// <summary>
        /// Diff selected file versus one of several options.
        /// The root menu class' Tag contains the name of the file to diff
        /// The individual menu item class Tag contains the diff options
        /// </summary>
        private void MenuItemDiffClick(object sender, EventArgs e)
        {
            string tag = (sender as ToolStripMenuItem).Tag.ToString();
            string diffOpt = tag;
            string fileName = contextMenuFile.Tag.ToString();
            ClassStatus status = App.Repos.Current.Status;
            status.Repo.GitDiff(diffOpt, new List<string> {fileName});
        }

        /// <summary>
        /// Show the revision history dialog for a selected file.
        /// This dialog is _not_ modal, so user can view multiple files.
        /// </summary>
        private void MenuItemRevClick(object sender, EventArgs e)
        {
            string file = contextMenuFile.Tag as string;
            var formRevisionHistory = new FormRevisionHistory(file) {Sha = Tag.ToString()};
            formRevisionHistory.Show();
        }

        #region Handle selected text copy

        /// <summary>
        /// Copy selected text onto the clipboard
        /// </summary>
        private void MenuItemCopyClick(object sender, EventArgs e)
        {
            textChangelist.Copy();
        }

        /// <summary>
        /// Handle key down event so we can do the copy (Ctrl + C)
        /// </summary>
        private void TextChangelistKeyDown(object sender, KeyEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.C)
            {
                MenuItemCopyClick(sender, null);
                e.SuppressKeyPress = true;
            }
        }

        #endregion
    }
}
