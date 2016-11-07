using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using GitForce.Main.Right.Panels;

namespace GitForce
{
    /// <summary>
    /// Class dialog showing a log of a single file.
    /// </summary>
    public partial class FormRevisionHistory : Form
    {
        /// <summary>
        /// The file name whose log this form shows
        /// </summary>
        private readonly string file;

        /// <summary>
        /// The current SHA string to initialize the list
        /// </summary>
        public string Sha { private get; set; }

        /// <summary>
        /// 2 last recently selected SHA submits
        /// </summary>
        private readonly string[] lruSha = new string[2];

        /// <summary>
        /// Form constructor. Takes the git file name whose history is to be shown.
        /// </summary>
        public FormRevisionHistory(string targetFile)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

            // Apply the same font we use for description of changes
            textDescription.Font = Properties.Settings.Default.commitFont;

            file = targetFile;
            Sha = String.Empty;
            Text = @"Revision History for //" + targetFile;
        }

        /// <summary>
        /// Access a virtual member
        /// </summary>
        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormRevisionHistoryFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// The form is loading. Get the file log information and fill it in.
        /// </summary>
        private void FormRevisionHistoryLoad(object sender, EventArgs e)
        {
            // Get the list of revisions by running a git command
            StringBuilder cmd = new StringBuilder("log ");

            cmd.Append(" --pretty=format:");        // Start formatting section
            cmd.Append("%h%x09");                   // Abbreviated commit hash
            cmd.Append("%ct%x09");                  // Committing time, UNIX-style
            cmd.Append("%an%x09");                  // Author name
            cmd.Append("%s");                       // Subject

            // Limit the number of commits to show
            if (Properties.Settings.Default.commitsRetrieveAll == false)
                cmd.Append(" -" + Properties.Settings.Default.commitsRetrieveLast);

            // Get the log of a single file only
            cmd.Append(" -- " + file);

            ExecResult result = App.Repos.Current.Run(cmd.ToString());
            if(result.Success())
                PanelRevlist.UpdateList(listRev, result.stdout);

            // Activate the given SHA item or the first one if none given
            int index = listRev.Items.IndexOfKey(Sha);
            if (index < 0)
                index = 0;
            listRev.SelectedIndices.Add(index);
            listRev.Items[index].EnsureVisible();
        }

        /// <summary>
        /// User clicked on a log item. Fetch its full description.
        /// </summary>
        private void ListRevSelectedIndexChanged(object sender, EventArgs e)
        {
            if(listRev.SelectedIndices.Count==0)
                return;

            // Set the menu enables according to the number of items selected
            viewMenuItem.Enabled = syncMenuItem.Enabled = diffVsClientFileMenuItem.Enabled = listRev.SelectedIndices.Count == 1;
            diffRevisionsMenuItem.Enabled = listRev.SelectedIndices.Count == 2;

            // Set up for 2 SHA checkins: the one in the [0] spot being the most recently selected
            if (listRev.SelectedIndices.Count == 1)
                lruSha[0] = lruSha[1] = listRev.SelectedItems[0].Text;
            if (listRev.SelectedIndices.Count > 1)
            {
                if (listRev.SelectedItems[0].Text == lruSha[0])
                    lruSha[1] = listRev.SelectedItems[1].Text;
                else
                    lruSha[1] = listRev.SelectedItems[0].Text;
            }

            if(listRev.SelectedIndices.Count==1)
            {
                string sha = lruSha[1];
                string cmd = string.Format("show -s {0}", sha);
                ExecResult result = App.Repos.Current.Run(cmd);
                textDescription.Text = result.Success() ? result.stdout : result.stderr;
            }
        }

        /// <summary>
        /// Close the dialog.
        /// </summary>
        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Diff selected against the HEAD revision
        /// </summary>
        private void DiffVsClientFileMenuItemClick(object sender, EventArgs e)
        {
            string cmd = "difftool " + ClassDiff.GetDiffCmd() + " " + lruSha[0] + "..HEAD -- " + file;
            RunDiff(cmd);
        }

        /// <summary>
        /// Diff 2 selected revisions
        /// </summary>
        private void DiffRevisionsMenuItemClick(object sender, EventArgs e)
        {
            string cmd = "difftool " + ClassDiff.GetDiffCmd() + " " + lruSha[0] + ".." + lruSha[1] + " -- " + file;
            RunDiff(cmd);
        }

        /// <summary>
        /// Runs a diff tool in the context of the current repo for a selected file.
        /// This is a separate function that runs a git command since we want to start a
        /// diff process and do not block.
        /// </summary>
        private void RunDiff(string cmd)
        {
            try
            {
                Process proc = new Process
                {
                    StartInfo =
                    {
                        FileName = Properties.Settings.Default.GitPath,
                        Arguments = cmd,
                        WorkingDirectory = App.Repos.Current.Root,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error executing diff", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sync file to the selected revision
        /// </summary>
        private void SyncMenuItemClick(object sender, EventArgs e)
        {
            if( MessageBox.Show("This will sync file to a previous version. Continue?", "Revision Sync",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                string cmd = string.Format("checkout {1} -- {0}", file, lruSha[0]);
                ExecResult result = App.Repos.Current.RunCmd(cmd);
                if(result.Success())
                    App.PrintStatusMessage("File checked out at a previous revision " + lruSha[0] + ": " + file, MessageType.General);
                else
                {
                    App.PrintStatusMessage("Sync error: " + result.stderr, MessageType.Error);
                    MessageBox.Show(result.stderr, "Sync error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// View file menu item is opening.
        /// Build a list of editors to view the selected file.
        /// </summary>
        private void ViewMenuItemDropDownOpening(object sender, EventArgs e)
        {
            viewMenuItem.DropDownItems.Clear();

            ToolStripMenuItem mEditAssoc = new ToolStripMenuItem("Associated Editor", null, MenuViewEditClick);
            viewMenuItem.DropDownItems.Add(mEditAssoc);
            string values = Properties.Settings.Default.EditViewPrograms;
            string[] progs = values.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                viewMenuItem.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(s), null, MenuViewEditClick) { Tag = s });
        }

        /// <summary>
        /// Handler for the view file menus.
        /// The tag of the sender specifies the operation on a selected file: null will open a file
        /// using the default association, while any other tag specifies a program to run.
        /// </summary>
        private void MenuViewEditClick(object sender, EventArgs e)
        {
            // Create a temp file on the selected git file version
            string temp = GetTempFile(file, listRev.SelectedItems[0].Text);
            if (!string.IsNullOrEmpty(temp))
                ClassUtils.FileOpenFromMenu(sender, temp);
        }

        /// <summary>
        /// Control is double-clicked. Open the selected item for viewing.
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        private void ListRevDoubleClick(object sender, EventArgs e)
        {
            if (listRev.SelectedIndices.Count == 1)
            {
                // Create a temp file and open the file
                string temp = GetTempFile(file, listRev.SelectedItems[0].Text);
                if (!string.IsNullOrEmpty(temp))
                    ClassUtils.FileDoubleClick(temp);
            }
        }

        /// <summary>
        /// Create a git file of a specific version. Use a temp file since the file
        /// content needs to be created from the git history.
        /// </summary>
        private string GetTempFile(string file, string sha)
        {
            // git show commands needs '/' as file path separator
            string gitpath = file.Replace(Path.DirectorySeparatorChar, '/');
            string cmd = string.Format("show {1}:\"{0}\"", gitpath, sha);

            ExecResult result = App.Repos.Current.Run(cmd);
            if (result.Success() == false)
                return string.Empty;

            string response = result.stdout;

            // Create a temp file based on our file and write content to it
            file = file.Replace(Path.DirectorySeparatorChar, '_');
            string temp = Path.Combine(Path.GetTempPath(), sha) + "_" + file;
            try
            {
                File.WriteAllText(temp, response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Add the temp file to the global list of temp files to be removed at the app exit time
            ClassGlobals.TempFiles.Add(temp);
            return temp;
        }
    }
}