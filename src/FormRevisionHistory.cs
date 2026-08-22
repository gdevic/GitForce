using System;
using System.Diagnostics;
using System.Linq;
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
        /// The file name whose log this form shows (relative to repo or submodule root)
        /// </summary>
        private readonly string file;

        /// <summary>
        /// The working directory for git commands (repo root or submodule path)
        /// </summary>
        private readonly string workingDir;

        /// <summary>
        /// True if the file is inside a submodule
        /// </summary>
        private readonly bool isInSubmodule;

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

            // Apply the same font we use for description of changes
            textDescription.Font = Properties.Settings.Default.commitFont;

            Sha = String.Empty;

            // Check if the file is inside a submodule
            workingDir = App.Repos.Current.Path;
            isInSubmodule = false;
            file = targetFile;

            if (App.Repos.Current.Submodules != null)
            {
                ClassSubmodules.Submodule sm;
                string relativePath;
                if (App.Repos.Current.Submodules.GetContainingSubmodule(targetFile, out sm, out relativePath))
                {
                    workingDir = sm.Path;
                    file = relativePath;
                    isInSubmodule = true;
                }
            }

            // Show complete path to the file being examined using the OS specific path separator
            Text = @"Revision History for " + App.Repos.Current.Path.Replace('\\', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + targetFile.Replace('\\', Path.DirectorySeparatorChar);
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
        /// Run a git command in the appropriate directory (repo root or submodule).
        /// </summary>
        private ExecResult RunGit(string args)
        {
            return App.Repos.Current.Run(args, false, isInSubmodule ? workingDir : null);
        }

        /// <summary>
        /// Returns this form's file name in the form a git command line needs: quoted, so a name
        /// containing spaces stays one argument, and with forward slashes, which "show sha:path"
        /// requires (it addresses a path inside a tree, where the separator is always '/', and
        /// rejects a backslash even on Windows). Pathspecs accept either separator, so one shape
        /// serves every command here.
        /// </summary>
        private string GitPath()
        {
            return "\"" + file.Replace(Path.DirectorySeparatorChar, '/') + "\"";
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
            cmd.Append(" -- " + GitPath());

            ExecResult result = RunGit(cmd.ToString());
            if (result.Success())
                PanelRevlist.UpdateList(listRev, result.stdout, true, string.Empty);
            if (listRev.Items.Count > 0)
            {
                // Activate the given SHA item or the first one if none given
                int index = listRev.Items.IndexOfKey(Sha);
                if (index < 0)
                    index = 0;
                listRev.SelectedIndices.Add(index);
                listRev.Items[index].EnsureVisible();
            }
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
            if (listRev.SelectedIndices.Count == 1)
            {
                string sha = lruSha[1];
                string cmd = string.Format("show -s {0}", sha);
                ExecResult result = RunGit(cmd);
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
            string cmd = "difftool " + ClassDiff.GetDiffCmd() + " " + lruSha[0] + "..HEAD -- " + GitPath();
            RunDiff(cmd);
        }

        /// <summary>
        /// Diff 2 selected revisions
        /// </summary>
        private void DiffRevisionsMenuItemClick(object sender, EventArgs e)
        {
            string cmd = "difftool " + ClassDiff.GetDiffCmd() + " " + lruSha[0] + ".." + lruSha[1] + " -- " + GitPath();
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
                        WorkingDirectory = workingDir,
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
            if (MessageBox.Show("This will sync file to a previous version. Continue?", "Revision Sync",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                string cmd = string.Format("checkout {1} -- {0}", GitPath(), lruSha[0]);
                ExecResult result = App.Repos.Current.RunCmd(cmd, false, isInSubmodule ? workingDir : null);
                if (result.Success())
                {
                    App.PrintStatusMessage("File checked out at a previous revision " + lruSha[0] + ": " + file, MessageType.General);
                    App.DoRefresh();
                }
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
            // Build the "View Using" submenus
            // The default option is to open the file using the OS-associated editor,
            // after which all the user-specified programs are listed
            ToolStripMenuItem mViewAssoc = new ToolStripMenuItem("Associated Editor", null, MenuViewEditClick) { };
            ToolStripMenuItem mView = new ToolStripMenuItem("View Using");
            mView.DropDownItems.Add(mViewAssoc);
            string values = Properties.Settings.Default.EditViewPrograms;
            string[] progs = values.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (progs.Any())
            {
                mView.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(progs[0]), null, MenuViewEditClick) { Tag = progs[0] });
                foreach (string s in progs.Skip(1))
                    mView.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(s), null, MenuViewEditClick) { Tag = s });
            }

            ToolStripMenuItem mDescribe = new ToolStripMenuItem("Describe Changelist...", null, MenuDescribeClick);
            ToolStripMenuItem mCopy = new ToolStripMenuItem("Copy SHA", null, MenuCopyShaClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mView, mDescribe, mCopy
            });

            if (listRev.SelectedIndices.Count != 1)
                mView.Enabled = mDescribe.Enabled = mCopy.Enabled = false;

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
            FormShowChangelist.DriveChangelistFromListViewEx(ref listRev);
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

            // Name the temp file after the revision index shown in the list. The row is
            // looked up by its SHA key, which may be missing, in which case the SHA itself
            // keeps the name unique.
            ListViewItem[] found = listRev.Items.Find(sha, false);
            string rev = found.Length > 0 ? found[0].Text.Trim() : sha;
            file = Path.GetFileName(file);
            file = string.Format("ReadOnly-{0}-Rev-{1}-{2}", tmpFileCounter, rev, file);
            tmpFileCounter++;
            string tempFile = Path.Combine(Path.GetTempPath(), file);

            if (!RunGitToFile(cmd, tempFile))
            {
                // Take the partially written file away. If something else is holding it,
                // hand it to the exit-time cleanup instead of abandoning it in %TEMP%.
                if (File.Exists(tempFile) && !ClassUtils.DeleteFile(tempFile))
                    ClassGlobals.TempFiles.Add(tempFile);
                return string.Empty;
            }

            // Add the temp file to the global list of temp files to be removed at the app exit time
            ClassGlobals.TempFiles.Add(tempFile);
            return tempFile;
        }

        /// <summary>
        /// Runs a git command and copies its standard output into a file, byte for byte.
        /// A revision of a file may well be binary, and a text file has to keep the line
        /// endings it was committed with. Neither survives the line based accumulation that
        /// the common Exec path performs, so this one writes the raw stream instead.
        /// Returns true if the file was written and git reported success.
        /// </summary>
        private bool RunGitToFile(string args, string targetFile)
        {
            Process proc = null;
            try
            {
                proc = new Process
                {
                    StartInfo =
                    {
                        FileName = Properties.Settings.Default.GitPath,
                        Arguments = args,
                        WorkingDirectory = workingDir,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                // Drain stderr on its own thread: filling that pipe while we are busy
                // copying stdout would block git and deadlock this call
                StringBuilder error = new StringBuilder();
                proc.ErrorDataReceived += (sender, e) => { if (e.Data != null) error.AppendLine(e.Data); };

                proc.Start();
                proc.BeginErrorReadLine();

                using (FileStream fs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                    proc.StandardOutput.BaseStream.CopyTo(fs);

                proc.WaitForExit();
                if (proc.ExitCode == 0)
                    return true;

                // git names the reason on stderr, but not for every failure, so say
                // something either way rather than failing without a word
                string message = error.ToString().Trim();
                App.PrintStatusMessage(string.IsNullOrEmpty(message)
                                           ? string.Format("git {0} returned {1}", args, proc.ExitCode)
                                           : message, MessageType.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // If we left through the exception path, git is still there writing into a
                // pipe that nobody reads any more. End it here: disposing the Process only
                // drops our handles, and the child would sit blocked until they finalize.
                if (proc != null)
                {
                    try
                    {
                        if (!proc.HasExited)
                            proc.Kill();
                    }
                    catch { }
                    try { proc.StandardOutput.Close(); }
                    catch { }
                    proc.Dispose();
                }
            }
        }
    }
}
