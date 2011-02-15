using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormMain_RightPanels
{
    public partial class PanelRevlist : UserControl
    {
        /// <summary>
        /// Branch name the log is using to display history
        /// </summary>
        private string _logBranch;

        public PanelRevlist()
        {
            InitializeComponent();

            App.Refresh += RevlistRefresh;
        }

        /// <summary>
        /// Fills in the list of revisions and changes to the repository
        /// </summary>
        private void RevlistRefresh()
        {
            listRev.BeginUpdate();
            listRev.Items.Clear();

            if (App.Repos.Current != null)
            {
                // Fill in available branch names
                ClassBranches branches = new ClassBranches();
                branches.Refresh();

                // Initialize our tracking branch
                if (string.IsNullOrEmpty(_logBranch))
                    _logBranch = branches.Current;

                List<string> allBranches = new List<string>();
                allBranches.AddRange(branches.Local);
                allBranches.AddRange(branches.Remote);
                btBranch.DropDownItems.Clear();
                foreach (string s in allBranches)
                {
                    var m = new ToolStripMenuItem(s) { Checked = s == _logBranch };
                    m.Click += LogBranchChanged;
                    btBranch.DropDownItems.Add(m);
                }

                // Get the list of revisions by running a git command
                StringBuilder cmd = new StringBuilder("log --pretty=format:\"");
                cmd.Append("%h%x09");       // Abbreviated commit hash
                cmd.Append("%ct%x09");      // Committing time, UNIX-style
                cmd.Append("%an%x09");      // Author name
                cmd.Append("%s");           // Subject
                // Add the branch name using only the first token in order to handle links (br -> br)
                cmd.Append("\" " + _logBranch.Split(' ').First());
                // Limit the number of commits to show
                if (Properties.Settings.Default.commitsRetrieveAll == false)
                    cmd.Append(" -" + Properties.Settings.Default.commitsRetrieveLast);

                string[] response = App.Git.Run(cmd.ToString()).Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in response)
                {
                    string[] cat = s.Split('\t');

                    // Convert the date/time from UNIX second based to C# date structure
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(cat[1])).ToLocalTime();
                    cat[1] = date.ToShortDateString() + " " + date.ToShortTimeString();

                    // Trim any spaces in the subject line
                    cat[3] = cat[3].Trim();

                    ListViewItem li = new ListViewItem(cat);
                    li.Tag = cat[0];            // Tag contains the SHA1 of the commit
                    listRev.Items.Add(li);
                }

                // Make columns auto-adjust to fit the width of the largest item
                foreach (ColumnHeader l in listRev.Columns) l.Width = -2;
            }

            listRev.EndUpdate();
        }

        /// <summary>
        /// Log branch changed
        /// </summary>
        private void LogBranchChanged(object sender, EventArgs e)
        {
            _logBranch = sender.ToString();
            RevlistRefresh();
        }

        /// <summary>
        /// Double-click on a changelist opens the describe changelist form
        /// </summary>
        private void ListRevMouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Get the SHA associated with the selected item on the log list
            ListView li = sender as ListView;
            if(li.SelectedIndices.Count!=1)
                return;
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
                if (dlg == DialogResult.No && index < li.Items.Count)
                    index++;

            } while (dlg != DialogResult.Cancel);
        }
    }
}
