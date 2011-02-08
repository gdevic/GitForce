using System;
using System.IO;
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
    public partial class PanelCommits : UserControl
    {
        /// <summary>
        /// Status class containing the git status of current repo files
        /// </summary>
        private ClassStatus Status;

        /// <summary>
        /// Class constructor
        /// </summary>
        public PanelCommits()
        {
            InitializeComponent();

            treeCommits.ImageList = ClassView.GetImageList();

            App.Refresh += CommitsRefresh;
        }

        /// <summary>
        /// Panel commit refresh function
        /// </summary>
        private void CommitsRefresh()
        {
            treeCommits.BeginUpdate();
            treeCommits.Nodes.Clear();

            if (App.Repos.Current != null)
            {
                Status = new ClassStatus(App.Repos.Current);

                // List files that are updated in the index (code X is not ' ' or '?')

                Status.SetListByCommand("status --porcelain -uno -z");
                Status.Filter(s => s[0] == ' ' || s[0] == '?');
                Status.Seal();

                // Build a list view of these files
                TreeNode node = ClassView.BuildCommitsView(Status.Repo, Status.GetFileList());

                // Add the resulting list to the tree view control
                treeCommits.Nodes.Add(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = (int)ClassView.Img.ChangeAll;

                // Assign the icons to the nodes of tree view
                ClassView.ViewAssignIcon(Status, node, true);

                // Always keep the root node expanded by default
                node.Expand();
            }
            treeCommits.EndUpdate();
        }

        /// <summary>
        /// Shortcut function to the panel refresh
        /// </summary>
        private void MenuRefreshClick(object sender, EventArgs e) { CommitsRefresh(); }

        /// <summary>
        /// User dragged something into the view
        /// </summary>
        private static void TreeCommitsDragEnter(object sender, DragEventArgs e)
        {
            // Allow drop only when there is a valid repo available
            if (App.Repos.Current != null)
                e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// Handler for the drop portion of drag and drop. User dropped one or more files to the commit pane.
        /// The files may originate from the left pane, or from an external application like explorer.
        /// </summary>
        private void TreeCommitsDragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            // Since files that are dropeed might have originated from anywhere, 
            // each file name needs to be compared against a set of git-permissible files for the current repo
            Status = new ClassStatus(App.Repos.Current);
            Status.SetListByCommand("status --porcelain -uall -z *");
            Status.Seal();

            // Qualify files as those that are in the scope of the current repo
            // Move files into different buckets based on what function needs to be done on them
            // Likely buckets are 'M' for modified files or 'A' for added files
            Dictionary<char, List<string>> opclass = new Dictionary<char, List<string>>();
            foreach (string s in files.Where(s => Status.IsMarked(s)))
            {
                if (opclass.ContainsKey(Status.GetYcode(s)))
                    opclass[Status.GetYcode(s)].Add("\"" + s + "\"");
                else
                    opclass[Status.GetYcode(s)] = new List<string> { "\"" + s + "\"" };
            }

            // Perform the required operation on the files
            if (opclass.ContainsKey('M'))
                Status.Repo.Run("add -- " + string.Join(" ", opclass['M']));

            if (opclass.ContainsKey('?'))
                Status.Repo.Run("add -- " + string.Join(" ", opclass['?']));

            if (opclass.ContainsKey('D'))
                Status.Repo.Run("rm -- " + string.Join(" ", opclass['D']));

            App.Refresh();
        }

        /// <summary>
        /// As the mouse moves over nodes, show the human readable description of
        /// files that the mouse points to
        /// </summary>
        private void TreeCommitsMouseMove(object sender, MouseEventArgs e)
        {
            if (Status != null)
                Status.ShowTreeInfo(treeCommits.GetNodeAt(e.X, e.Y));
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void TreeCommitsMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode sel = treeCommits.GetNodeAt(e.X, e.Y) ?? treeCommits.Nodes[0];

                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, sel.Tag));

                // Add the Refresh (F5) menu item
                ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);
                contextMenu.Items.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRefresh });
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for commits
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, object tag)
        {
            // Menus are set in this order:
            //  [0]  Diff vs Repo HEAD
            //  [0]  Submit...           -> when there are files in the selected bundle
            //  [1]  New Commit...       -> always open a dialog
            //  [2]  Edit Commit...      -> enabled if the tag is ClassCommit                 and not Default
            //  [3]  Remove Empty Commit -> enabled if the tag is ClassCommit and it is empty and not Default

            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs Repo HEAD", null, MenuDiffClick);
            mDiff.Tag = tag;
            ToolStripMenuItem mSub = new ToolStripMenuItem("Submit...", null, MenuSubmitClick, Keys.Control | Keys.S);
            mSub.Tag = tag;
            ToolStripMenuItem mNew = new ToolStripMenuItem("New Changelist...", null, MenuNewCommitClick);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Spec...", null, MenuEditCommitClick);
            mEdit.Tag = tag;        // Store the ClassCommit in the menu tag
            ToolStripMenuItem mDel  = new ToolStripMenuItem("Delete Empty Changelist", null, MenuDeleteEmptyClick);
            mDel.Tag = tag;         // Store the ClassCommit in the menu tag
            ToolStripMenuItem mRevert = new ToolStripMenuItem("Revert", null, MenuRevertClick);
            mRevert.Tag = tag;

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mDiff, mSub, 
                new ToolStripSeparator(),
                mNew, mEdit, mDel,
                new ToolStripSeparator(),
                mRevert
            });

            if (treeCommits.Nodes.Count==0 || tag == treeCommits.Nodes[0].Tag)
                mSub.Enabled = false;

            if (App.Repos.Current == null)
                mNew.Enabled = false;

            if (!(tag is ClassCommit && !(tag as ClassCommit).IsDefault))
                mEdit.Enabled = false;

            if (!(tag is ClassCommit && (tag as ClassCommit).Files.Count == 0 && !(tag as ClassCommit).IsDefault))
                mDel.Enabled = false;

            return menu;
        }

        /// <summary>
        /// New commit bundle needed
        /// </summary>
        private void MenuNewCommitClick(object sender, EventArgs e)
        {
            FormCommit commitForm = new FormCommit(false);
            commitForm.SetFiles(Status.Repo.Commits.Bundle[0]);
            if (commitForm.ShowDialog() == DialogResult.OK)
            {
                // Create a new commit bundle and move selected files to it
                Status.Repo.Commits.NewBundle(commitForm.GetDescription(), commitForm.GetFiles());
                CommitsRefresh();
            }
        }

        /// <summary>
        /// Edit selected commit bundle
        /// </summary>
        private void MenuEditCommitClick(object sender, EventArgs e)
        {
            // Recover the commit class bundle that was selected
            ClassCommit c = (sender as ToolStripMenuItem).Tag as ClassCommit;

            FormCommit commitForm = new FormCommit(false, c.Description);
            commitForm.SetFiles(c);
            if (commitForm.ShowDialog() == DialogResult.OK)
            {
                // Update the description text and the list of files
                c.Description = commitForm.GetDescription();

                // Renew only files that were left checked, the rest add back to the Default commit
                List<string> removedFiles = c.Renew(commitForm.GetFiles());
                Status.Repo.Commits.Bundle[0].AddFiles(removedFiles);
                CommitsRefresh();
            }
        }

        /// <summary>
        /// Submit selected files within a changelist
        /// </summary>
        private void MenuSubmitClick(object sender, EventArgs e)
        {
            // If the right-click selected a changelist bundle, submit it
            // All the files that were selected should be checked in the list
            ClassCommit c;
            if ((sender as ToolStripMenuItem).Tag is ClassCommit)
                c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            else
            {
                // If the right-click selected files, gather all files selected
                // into a pseudo-bundle to submit
                c = new ClassCommit("ad-hoc");
                List<string> files = (from n in treeCommits.SelectedNodes 
                                      where Status.IsMarked(n.Tag.ToString()) 
                                      select n.Tag.ToString()).ToList();
                c.AddFiles(files);
            }

            FormCommit commitForm = new FormCommit(true, c.Description);
            commitForm.SetFiles(c);
            if (commitForm.ShowDialog() == DialogResult.OK)
            {
                // Get the files checked for commit
                List<string> final = commitForm.GetFiles();
                if (final.Count > 0)
                {
                    // Create a temp file to store our commit message
                    string tempFile = Path.GetTempFileName();
                    File.WriteAllText(tempFile, commitForm.GetDescription());

                    // Form the final command with the description file, optional amend and
                    // the final list of files which are then quoted
                    string cmd = "commit -F " + tempFile +
                        (commitForm.GetCheckAmend()? " --amend -- " : " -- ") +
                        String.Join(" ", final.Select(s => "\"" + s + "\"").ToList());

                    Status.Repo.Run(cmd);

                    File.Delete(tempFile);

                    // If the current commit bundle is not default, remove it. Refresh which follows
                    // will reset all files which were _not_ submitted as part of this change to be
                    // moved to the default changelist.
                    if (!c.IsDefault)
                    {
                        App.Repos.Current.Commits.Bundle.Remove(c);
                    }
                }
                App.Refresh();
            }
        }

        /// <summary>
        /// Delete empty commit bundle.
        /// It is already verified that the commit bundle (sent in the tag) is empty.
        /// </summary>
        private void MenuDeleteEmptyClick(object sender, EventArgs e)
        {
            // Recover the commit class bundle that was selected
            ClassCommit c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            Status.Repo.Commits.Bundle.Remove(c);
            CommitsRefresh();
        }

        /// <summary>
        /// Diff the selected file against the repo
        /// </summary>
        private void MenuDiffClick(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Tag is string)
            {
                string file = (sender as ToolStripMenuItem).Tag as string;
                file = file.Substring(Status.Repo.Root.Length + 1);
                string cmd = "difftool --cached -- \"" + file + "\"";
                Status.Repo.Run(cmd);
                App.Refresh();
            }
        }

        /// <summary>
        /// Revert selected files or a submit bundle
        /// </summary>
        private void MenuRevertClick(object sender, EventArgs e)
        {
            // If the right-click selected a changelist bundle, revert it
            // Use the class commit as a helper class to hold a set of files
            ClassCommit c;
            if ((sender as ToolStripMenuItem).Tag is ClassCommit)
            {
                c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            }
            else
            {
                // If the right-click selected files, gather all files selected
                // into a pseudo-bundle to submit
                c = new ClassCommit("ad-hoc");
                List<string> files = (from n in treeCommits.SelectedNodes 
                                      where Status.IsMarked(n.Tag.ToString()) 
                                      select n.Tag.ToString()).ToList();
                c.AddFiles(files);
            }

            // Get the files checked for commit
            if (c.Files.Count > 0)
            {
                if (MessageBox.Show(@"Revert will unstage all the selected files and will lose the changes.
Proceed with Revert?", "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    string cmd = "reset HEAD -- " +
                    String.Join(" ", c.Files.Select(s => "\"" + s + "\"").ToList());
                    Status.Repo.Run(cmd);
                    App.Refresh();
                }
            }
        }
    }
}
