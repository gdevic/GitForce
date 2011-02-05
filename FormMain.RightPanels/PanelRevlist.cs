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
        public PanelRevlist()
        {
            InitializeComponent();

            App.Refresh += revlistRefresh;
        }

        /// <summary>
        /// Fills in the list of revisions and changes to the repository
        /// </summary>
        private void revlistRefresh()
        {
            listRev.BeginUpdate();
            listRev.Items.Clear();

            if (App.Repos.current != null)
            {
                // Get the list of revisions by running a git command
                StringBuilder cmd = new StringBuilder("log --pretty=format:\"");
                cmd.Append("%h%x09");       // Abbreviated commit hash
                cmd.Append("%ct%x09");      // Committing time, UNIX-style
                cmd.Append("%cn%x09");      // Committer name
                cmd.Append("%s");           // Subject
                cmd.Append("\" HEAD");
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
    }
}
