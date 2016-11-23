using System;
using System.ComponentModel;
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
        /// Temp file counter number
        /// </summary>
        private int tmpFileCounter = 1;

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

            // Show complete path to the file being examined using the OS specific path separator
            Text = @"Revision History for " + App.Repos.Current.Root.Replace('\\', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + targetFile.Replace('\\', Path.DirectorySeparatorChar);
            // If the path specifies a folder (for example, user clicked on the root repo name on the view pane), add "..."
            if (Text[Text.Length - 1] == Path.DirectorySeparatorChar)
                Text = Text + "...";
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
                PanelRevlist.UpdateList(listRev, result.stdout, true);

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
            // Set the menu enables according to the number of items selected
            viewMenuItem.Enabled = syncMenuItem.Enabled = diffVsClientFileMenuItem.Enabled = listRev.SelectedIndices.Count == 1;
            diffRevisionsMenuItem.Enabled = listRev.SelectedIndices.Count == 2;

            // Set up for 2 SHA checkins: the one in the [0] spot being the most recently selected
            if (listRev.SelectedIndices.Count == 1)
                lruSha[0] = lruSha[1] = listRev.SelectedItems[0].Name;
            if (listRev.SelectedIndices.Count > 1)
            {
                if (listRev.SelectedItems[0].Name == lruSha[0])
                    lruSha[1] = listRev.SelectedItems[1].Name;
                else
                    lruSha[1] = listRev.SelectedItems[0].Name;
            }
            // Fill in the description of a selected checkin if a single one is selected
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
            string temp = GetTempFile(file, listRev.SelectedItems[0].Name);
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
                string temp = GetTempFile(file, listRev.SelectedItems[0].Name);
                if (!string.IsNullOrEmpty(temp))
                    ClassUtils.FileDoubleClick(temp);
            }
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void ListRevMouseUp(object sender, MouseEventArgs e)
        {
            // Clear the context menu first so it's only shown when we enter the condition below
            contextMenu.Items.Clear();
            if (e.Button == MouseButtons.Right && listRev.SelectedIndices.Count > 0)
            {
                // Build the context menu to be shown
                contextMenu.Items.AddRange(GetContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Builds and returns a context menu for revision history list
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            ToolStripMenuItem mDescribe = new ToolStripMenuItem("Describe Changelist...", null, MenuDescribeClick);
            ToolStripMenuItem mCopy = new ToolStripMenuItem("Copy SHA", null, MenuCopyShaClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mDescribe, mCopy
            });

            if (listRev.SelectedIndices.Count != 1)
                mDescribe.Enabled = mCopy.Enabled = false;

            return menu;
        }

        /// <summary>
        /// Copy the selected SHA number into the clipboard
        /// </summary>
        private void MenuCopyShaClick(object sender, EventArgs e)
        {
            if (listRev.SelectedIndices.Count == 1)
            {
                string sha = listRev.SelectedItems[0].Name;
                Clipboard.SetText(sha);
            }
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
            li.MultiSelect = true;
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

            // Create a temp file based on a version of our file and write its content to it
            string rev = listRev.Items.Find(sha, false)[0].Text.Trim();
            file = Path.GetFileName(file);
            file = string.Format("ReadOnly-{0}-Rev-{1}-{2}", tmpFileCounter, rev, file);
            tmpFileCounter++;
            string tempFile = Path.Combine(Path.GetTempPath(), file);
            try
            {
                File.WriteAllText(tempFile, response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Add the temp file to the global list of temp files to be removed at the app exit time
            ClassGlobals.TempFiles.Add(tempFile);
            return tempFile;
        }
    }
}