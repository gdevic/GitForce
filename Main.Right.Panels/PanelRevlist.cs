using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Main.Right.Panels
{
    public partial class PanelRevlist : UserControl
    {
        /// <summary>
        /// Use static filter form class in order to preserve fields during the application run
        /// and to be able to get the current filtering string out of it at any time.
        /// </summary>
        readonly FormChangelistFilter formFilter = new FormChangelistFilter();

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
        public void RevlistRefresh()
        {
            // Clear the current lists in preparation for the refresh
            listRev.Items.Clear();
            btBranch.DropDownItems.Clear();
            labelLogBranch.Text = "";

            if (App.Repos.Current != null)
            {
                ClassBranches branches = App.Repos.Current.Branches;

                // Initialize our tracking branch
                if (!branches.Local.Contains(_logBranch) && !branches.Remote.Contains(_logBranch))
                    _logBranch = branches.Current;

                // If the repo does not have a branch at all (new repo that was just initialized), exit
                if (string.IsNullOrEmpty(_logBranch))
                    return;

                if (_logBranch != branches.Current)
                    labelLogBranch.Text = String.Format(" (Branch: \"{0}\")", _logBranch);

                // Populate the drop-down list of branches: local and remote)
                foreach (var branch in branches.Local)
                    btBranch.DropDownItems.Add(new ToolStripMenuItem(branch, null, LogBranchChanged));

                foreach (var branch in branches.Remote)
                    btBranch.DropDownItems.Add(new ToolStripMenuItem(branch, null, LogBranchChanged));

                // Get the list of revisions by running a git command
                StringBuilder cmd = new StringBuilder("log ");

                // If we are filtering, append the filter string
                if (btClearFilter.Enabled)
                    cmd.Append(formFilter.gitFilter);

                cmd.Append(" --pretty=format:");        // Start formatting section
                cmd.Append("%h%x09");                   // Abbreviated commit hash
                cmd.Append("%ct%x09");                  // Committing time, UNIX-style
                cmd.Append("%an%x09");                  // Author name
                cmd.Append("%s");                       // Subject
                // Add the branch name using only the first token in order to handle links (br -> br)
                if(_logBranch!="(no branch)")
                    cmd.Append(" " + _logBranch.Split(' ').First());
                // Limit the number of commits to show
                if (Properties.Settings.Default.commitsRetrieveAll == false)
                    cmd.Append(" -" + Properties.Settings.Default.commitsRetrieveLast);

                ExecResult result = App.Repos.Current.Run(cmd.ToString());
                if(result.Success())
                    UpdateList(listRev, result.stdout);
            }
        }

        /// <summary>
        /// Helper function that fills in the list of revisions.
        /// This is used from the code above and from the FormRevisionHistory.
        /// </summary>
        public static void UpdateList(ListView listRev, string input)
        {
            App.StatusBusy(true);
            string[] response = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            listRev.BeginUpdate();
            listRev.Items.Clear();

            foreach (string s in response)
            {
                string[] cat = s.Split('\t');

                // Convert the date/time from UNIX second based to C# date structure
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(cat[1])).ToLocalTime();
                cat[1] = String.Format("{0:yyyy/MM/dd  HH:mm:ss}", date);

                // Trim any spaces in the subject line
                cat[3] = cat[3].Trim();
                // Limit the subject line length to the length specified for that
                int c1 = Convert.ToInt32(Properties.Settings.Default.commitW1);
                if (cat[3].Length > c1)
                    cat[3] = cat[3].Substring(0, c1) + "...";

                ListViewItem li = new ListViewItem(cat);
                li.Name = cat[0];           // Used to search for a key
                li.Tag = cat[0];            // Tag contains the SHA1 of the commit
                listRev.Items.Add(li);
            }

            // Make columns auto-adjust to fit the width of the largest item
            foreach (ColumnHeader l in listRev.Columns) l.Width = -2;

            listRev.EndUpdate();
            App.StatusBusy(false);
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
            MenuDescribeClick(sender, null);
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void ListRevMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listRev.Items.Count > 0)
            {
                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu));

                // Add the Refresh (F5) menu item
                ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);
                contextMenu.Items.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRefresh });
            }
        }

        /// <summary>
        /// Builds and returns a context menu for revision list
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            ToolStripMenuItem mDescribe = new ToolStripMenuItem("Describe Changelist", null, MenuDescribeClick);
            ToolStripMenuItem mReset = new ToolStripMenuItem("Reset", null, MenuResetClick);
            ToolStripMenuItem mCherry = new ToolStripMenuItem("Cherry pick", null, MenuCherryPickClick);
            ToolStripMenuItem mCopy = new ToolStripMenuItem("Copy SHA", null, MenuCopyShaClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mDescribe, mReset, mCherry, 
                new ToolStripSeparator(),
                mCopy
            });

            // Enable menu items only if there was a change selected
            mDescribe.Enabled = mReset.Enabled = mCherry.Enabled = mCopy.Enabled = GetSelectedSha() != null;

            return menu;
        }

        /// <summary>
        /// Get the SHA associated with the selected item on the log list
        /// Returns null if the unique SHA cannot be obtained.
        /// </summary>
        private string GetSelectedSha()
        {
            ListView li = listRev;
            if (li.SelectedIndices.Count != 1)
                return null;
            int index = li.SelectedIndices[0];
            return li.Items[index].Tag.ToString();
        }

        /// <summary>
        /// Describe (view) selected changelist
        /// </summary>
        private void MenuDescribeClick(object sender, EventArgs e)
        {
            // Get the SHA associated with the selected item on the log list
            ListView li = listRev;
            if (li.SelectedIndices.Count != 1)
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
                if (dlg == DialogResult.No && index < li.Items.Count - 1)
                    index++;

            } while (dlg != DialogResult.Cancel);
        }

        /// <summary>
        /// Reset current branch to the selected submit
        /// </summary>
        private void MenuResetClick(object sender, EventArgs e)
        {
            string sha = GetSelectedSha();
            if(sha!=null)
            {
                FormReset formReset = new FormReset();
                if( formReset.ShowDialog()==DialogResult.OK)
                {
                    string cmd = String.Format("reset {0} {1}", formReset.cmd, sha);
                    App.Repos.Current.RunCmd(cmd);
                    App.DoRefresh();                    
                }
            }
        }

        /// <summary>
        /// Cherry pick selected submit
        /// </summary>
        private void MenuCherryPickClick(object sender, EventArgs e)
        {
            string sha = GetSelectedSha();
            if (sha != null)
            {
                string cmd = "cherry-pick --no-commit " + sha;
                App.Repos.Current.RunCmd(cmd);
                App.DoRefresh();
            }
        }

        /// <summary>
        /// Copy the selected SHA number into the clipboard
        /// </summary>
        private void MenuCopyShaClick(object sender, EventArgs e)
        {
            string sha = GetSelectedSha();
            if (sha != null)
            {
                Clipboard.SetText(sha);
            }
        }

        /// <summary>
        /// Shortcut function to the panel refresh
        /// </summary>
        private void MenuRefreshClick(object sender, EventArgs e) { RevlistRefresh(); }

        /// <summary>
        /// Set the log filter using the custom dialog
        /// </summary>
        private void MenuSetFilterClick(object sender, EventArgs e)
        {
            if(formFilter.ShowDialog()==DialogResult.OK)
            {
                // btClearFilter enable is keeping the flag if we are filtering or not
                btClearFilter.Enabled = true;
                RevlistRefresh();
            }
        }

        /// <summary>
        /// Clear the log filter
        /// </summary>
        private void MenuClearFilterClick(object sender, EventArgs e)
        {
            // btClearFilter enable is keeping the flag if we are filtering or not
            btClearFilter.Enabled = false;
            RevlistRefresh();
        }
    }
}
