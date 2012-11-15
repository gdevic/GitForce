using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Main.Right.Panels
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
        public void CommitsRefresh()
        {
            treeCommits.BeginUpdate();
            treeCommits.NodesClear();

            if (App.Repos.Current != null)
            {
                Status = App.Repos.Current.Status;

                // List files that are updated in the index (code X is not ' ' or '?')
                List<string> files = Status.GetFiles();
                files = files.Where(s => (Status.Xcode(s) != ' ' && Status.Xcode(s) != '?')).ToList();

                // Build a list view of these files
                TreeNode node = ClassView.BuildCommitsView(Status.Repo, files);

                // Add the resulting list to the tree view control
                treeCommits.Nodes.Add(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = (int)ClassView.Img.ChangelistRoot;

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
        /// Returns a set of selected git files
        /// </summary>
        private List<string> GetSelectedFiles()
        {
            List<string> files = (from n in treeCommits.SelectedNodes
                                  where Status.IsMarked(n.Tag.ToString())
                                  select n.Tag.ToString()).ToList();
            return files;
        }

        /// <summary>
        /// User selected one or more items and started to drag them
        /// </summary>
        private void TreeCommitsItemDrag(object sender, ItemDragEventArgs e)
        {
            // Get the list of selected files, prepend the repo root path and send it as a drag/drop list of files
            string[] files = treeCommits.SelectedNodes.Select(s => Path.Combine(Status.Repo.Root, s.Tag.ToString())).ToArray();
            DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Copy);
        }

        /// <summary>
        /// User dragged something into the view
        /// </summary>
        private void TreeCommitsDragEnter(object sender, DragEventArgs e)
        {
            // Allow drop only when there is a valid repo available
            if (App.Repos.Current != null)
                e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// Adds a list of files to the default commit (index)
        /// This static function may also be used by callers wishing to update index.
        /// files is a list of files with relative paths.
        /// </summary>
        public static void DoDropFiles(ClassStatus status, List<string> files)
        {
            // Qualify files as those that are in the scope of the current repo
            // Move files into different buckets based on what function needs to be done on them
            Dictionary<char, List<string>> opclass = new Dictionary<char, List<string>>();
            foreach (string s in files.Where(status.IsMarked))
            {
                if (opclass.ContainsKey(status.Ycode(s)))
                    opclass[status.Ycode(s)].Add(s);
                else
                    opclass[status.Ycode(s)] = new List<string> { s };
            }

            // Perform required operations on the files
            if (opclass.ContainsKey('?'))
                status.Repo.GitAdd(opclass['?']);
            if (opclass.ContainsKey('M'))
                status.Repo.GitUpdate(opclass['M']);
            if (opclass.ContainsKey('D'))
                status.Repo.GitDelete(opclass['D']);
            if (opclass.ContainsKey('R'))
                status.Repo.GitRename(opclass['R']);
        }

        /// <summary>
        /// Handler for the drop portion of drag and drop. User dropped one or more files to the commit pane.
        /// The files may originate from the left pane, or from an external application like explorer.
        /// </summary>
        private void TreeCommitsDragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Files can come from anywhere. Prune those that are not from this repo.
            List<string> files = (from file in droppedFiles.ToList()
                                  where file.StartsWith(Status.Repo.Root)
                                  select file.Substring(Status.Repo.Root.Length + 1)).ToList();

            DoDropFiles(Status, files.ToList());

            // Find which node the files have been dropped to, so we can
            // move them to the selected commit bundle

            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode destNode = ((TreeView) sender).GetNodeAt(pt);
            if (destNode!=null)
            {
                // Destination node could be a bundle root, or a file node (with a parent root)
                ClassCommit bundle = null;
                if (destNode.Tag is ClassCommit)
                    bundle = destNode.Tag as ClassCommit;
                if (destNode.Tag is string && destNode.Tag.ToString() != "root")
                    bundle = destNode.Parent.Tag as ClassCommit;

                // If we have a valid class bundle where the files should be moved to, move them
                if(bundle!=null)
                {
                    List<string> list = files.Where(s => Status.IsMarked(s)).ToList();
                    App.Repos.Current.Commits.MoveOrAdd(bundle, list);
                }
            }
            App.DoRefresh();
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
            if (e.Button == MouseButtons.Right && treeCommits.Nodes.Count > 0)
            {
                TreeNode sel = treeCommits.GetNodeAt(e.X, e.Y) ?? treeCommits.Nodes[0];

                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, sel.Tag));

                // Add the Refresh (F5) menu item
                ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);
                contextMenu.Items.AddRange(new ToolStripItem[] {new ToolStripSeparator(), mRefresh});
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for commits
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, object tag)
        {
            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs Repo HEAD", null, MenuDiffClick) {Tag = tag};
            ToolStripMenuItem mSub = new ToolStripMenuItem("Submit...", null, MenuSubmitClick, Keys.Control | Keys.S) {Tag = tag};
            ToolStripMenuItem mNew = new ToolStripMenuItem("New Changelist...", null, MenuNewCommitClick);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Spec...", null, MenuEditCommitClick) {Tag = tag};
            ToolStripMenuItem mDel = new ToolStripMenuItem("Delete Empty Changelist", null, MenuDeleteEmptyClick) {Tag = tag};
            ToolStripMenuItem mRevert = new ToolStripMenuItem("Revert", null, MenuRevertClick) {Tag = tag};

            // Build the "Resolve" submenu
            ToolStripMenuItem mResolveTool = new ToolStripMenuItem("Run Merge Tool...", null, MenuMergeTool) { Tag = tag };
            ToolStripMenuItem mResolve = new ToolStripMenuItem("Resolve");
            mResolve.DropDownItems.AddRange(new ToolStripItem[] { mResolveTool });

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mDiff, mSub, 
                new ToolStripSeparator(),
                mNew, mEdit, mDel,
                new ToolStripSeparator(),
                mRevert,
                mResolve
            });

            if (treeCommits.Nodes.Count == 0 || tag == treeCommits.Nodes[0].Tag)
                mSub.Enabled = false;

            if (App.Repos.Current == null)
                mDiff.Enabled = mNew.Enabled = mRevert.Enabled = false;

            if (!(tag is ClassCommit))
                mEdit.Enabled = false;

            if (!(tag is ClassCommit && (tag as ClassCommit).Files.Count == 0 && !(tag as ClassCommit).IsDefault))
                mDel.Enabled = false;

            if (!((tag is string) && Status.IsMarked(tag as string) && Status.Xcode(tag as string) == 'U'))
                mResolve.Enabled = false;

            return menu;
        }

        /// <summary>
        /// New commit bundle needed
        /// </summary>
        private void MenuNewCommitClick(object sender, EventArgs e)
        {
            FormCommit commitForm = new FormCommit(false, "Update");
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
                // Unless we are amending, there should be at least one file selected
                if (final.Count > 0 || commitForm.GetCheckAmend())
                {
                    // Create a temp file to store our commit message
                    string tempFile = Path.GetTempFileName();
                    File.WriteAllText(tempFile, commitForm.GetDescription());

                    // Form the final command with the description file and an optional amend
                    Status.Repo.GitCommit("-F \"" + tempFile + "\"", commitForm.GetCheckAmend(), final);

                    File.Delete(tempFile);

                    // If the current commit bundle is not default, remove it. Refresh which follows
                    // will reset all files which were _not_ submitted as part of this change to be
                    // moved to the default changelist.
                    if (!c.IsDefault)
                        App.Repos.Current.Commits.Bundle.Remove(c);
                    else
                        c.Description = "Default";
                }
                App.DoRefresh();
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
        /// Diff selected files against the repo
        /// </summary>
        private void MenuDiffClick(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Tag is string)
            {
                List<string> files = GetSelectedFiles();
                Status.Repo.GitDiff("--cached", files);
            }
        }

        /// <summary>
        /// Revert selected files or a submit bundle
        /// </summary>
        private void MenuRevertClick(object sender, EventArgs e)
        {
            List<string> files;

            // If the right-click selected a changelist bundle, revert it
            // If the right-click selected files, gather all files selected
            if ((sender as ToolStripMenuItem).Tag is ClassCommit)
                files = ((sender as ToolStripMenuItem).Tag as ClassCommit).Files;
            else
                files = GetSelectedFiles();

            // Get the files checked for commit
            if (files.Count <= 0) return;

            if (MessageBox.Show(@"Revert will unstage all the selected files and will lose the changes.Proceed with Revert?",
                    "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes) return;

            // Renamed files are handled first
            // Simply rename them back to their original names
            List<string> used = new List<string>();
            foreach (var s in files)
            {
                string alt = Status.GetAltFile(s);
                if (alt == string.Empty) continue;
                Status.Repo.GitMove(s, alt);
                used.Add(s);
            }
            files = files.Except(used).ToList();

            // With whatever is left...
            if(files.Count>0)
            {
                // There are 2 ways to unstage a file:
                // https://git.wiki.kernel.org/index.php/GitFaq#Why_is_.22git_rm.22_not_the_inverse_of_.22git_add.22.3F
                // Can't figure out how to find out which one to use at this moment, so use both.
                if (Status.Repo.GitReset("HEAD", files) == true)
                {
                    App.PrintStatusMessage("Retrying using the `rm` option...");
                    Status.Repo.GitDelete("--cached", files);
                }
            }
            App.DoRefresh();
        }

        /// <summary>
        /// Run the merge tool
        /// </summary>
        private void MenuMergeTool(object sender, EventArgs e)
        {
            string file = (sender as ToolStripMenuItem).Tag.ToString();
            string cmd = "mergetool " + ClassMerge.GetMergeCmd() + " \"" + file + "\"";
            Status.Repo.RunCmd(cmd);
            App.DoRefresh();
        }
    }
}
