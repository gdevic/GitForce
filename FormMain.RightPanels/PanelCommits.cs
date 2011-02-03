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
        /// Status class containing the git status of the files
        /// </summary>
        private ClassStatus Status = null;

        /// <summary>
        /// Current repo, another instance
        /// </summary>
        ClassRepo repo = null;

        /// <summary>
        /// Class constructor
        /// </summary>
        public PanelCommits()
        {
            InitializeComponent();

            treeCommits.ImageList = ClassStatus.GetImageList();

            App.Refresh += commitsRefresh;
        }

        /// <summary>
        /// Panel commit refresh function
        /// </summary>
        private void commitsRefresh()
        {
            treeCommits.BeginUpdate();
            treeCommits.Nodes.Clear();

            if (App.Repos.current != null)
            {
                repo = App.Repos.current;
                Status = new ClassStatus(repo);

                // List files that are updated in the index (code X is not ' ' or '?')

                Status.SetListByCommand("status --porcelain -uno -z *");
                Status.Filter(delegate(string s) { return s[0] == ' ' || s[0] == '?'; });
                Status.Seal();

                // Build a list view of these files
                TreeNode node = Status.BuildCommitsView();

                // Add the resulting list to the tree view control
                treeCommits.Nodes.Add(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = (int)ClassStatus.Img.CHANGE_ALL;

                // Assign the icons to the nodes of tree view
                Status.viewAssignIcon(node, isIndex:true);

                // Always keep the root node expanded by default
                node.Expand();
            }
            treeCommits.EndUpdate();
        }

        /// <summary>
        /// Shortcut function to the panel refresh
        /// </summary>
        private void menuRefresh_Click(object sender, EventArgs e) { commitsRefresh(); }

        /// <summary>
        /// User dragged something into the view
        /// </summary>
        private void treeCommits_DragEnter(object sender, DragEventArgs e)
        {
            // Allow drop only when there is a valid repo available
            if (App.Repos.current != null)
                e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// Handler for the drop portion of drag and drop. User dropped one or more files to the commit pane.
        /// The files may originate from the left pane, or from an external application like explorer.
        /// </summary>
        private void treeCommits_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Since files that are dropeed might have originated from anywhere, 
            // each file name needs to be compared against a set of git-permissible files for the current repo
            Status = new ClassStatus(App.Repos.current);
            Status.SetListByCommand("status --porcelain -uall -z *");
            Status.Seal();

            // Qualify files as those that are in the scope of the current repo
            // Move files into different buckets based on what function needs to be done on them
            // Likely buckets are 'M' for modified files or 'A' for added files
            Dictionary<char, List<string>> opclass = new Dictionary<char, List<string>>();
            foreach (string s in files)
                if (Status.isMarked(s))
                    if (opclass.ContainsKey(Status.GetY(s)))
                        opclass[Status.GetY(s)].Add("\"" + s + "\"");
                    else
                        opclass[Status.GetY(s)] = new List<string>() { "\"" + s + "\"" };

            // Perform the required operation on the files
            if (opclass.ContainsKey('M'))
                App.Git.Run("add -- " + string.Join(" ", opclass['M']));

            if (opclass.ContainsKey('?'))
                App.Git.Run("add -- " + string.Join(" ", opclass['?']));

            if (opclass.ContainsKey('D'))
                App.Git.Run("rm -- " + string.Join(" ", opclass['D']));

            App.Refresh();
        }

        /// <summary>
        /// As the mouse moves over nodes, show the human readable description of
        /// files that the mouse points to
        /// </summary>
        private void treeCommits_MouseMove(object sender, MouseEventArgs e)
        {
            if (Status != null)
                Status.ShowTreeInfo(treeCommits.GetNodeAt(e.X, e.Y));
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void treeCommits_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode sel = treeCommits.GetNodeAt(e.X, e.Y);
                if (sel == null)
                    sel = treeCommits.Nodes[0];

                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, sel.Tag));

                // Add the Refresh (F5) menu item
                ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, menuRefresh_Click, Keys.F5);
                contextMenu.Items.AddRange(new ToolStripItem[] { new ToolStripSeparator(), mRefresh });
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for commits
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, object tag)
        {
            // Menus are set in this order:
            //  [0]  Diff File Against Repo...
            //  [0]  Submit...           -> when there are files in the selected bundle
            //  [1]  New Commit...       -> always open a dialog
            //  [2]  Edit Commit...      -> enabled if the tag is ClassCommit                 and not Default
            //  [3]  Remove Empty Commit -> enabled if the tag is ClassCommit and it is empty and not Default

            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff File Against Repo...", null, menuDiff_Click);
            mDiff.Tag = tag;
            ToolStripMenuItem mSub = new ToolStripMenuItem("Submit...", null, menuSubmit_Click, Keys.Control | Keys.S);
            mSub.Tag = tag;
            ToolStripMenuItem mNew = new ToolStripMenuItem("New Changelist...", null, menuNewCommit_Click);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Spec...", null, menuEditCommit_Click);
            mEdit.Tag = tag;        // Store the ClassCommit in the menu tag
            ToolStripMenuItem mDel  = new ToolStripMenuItem("Delete Empty Changelist", null, menuDeleteEmpty_Click);
            mDel.Tag = tag;         // Store the ClassCommit in the menu tag
            ToolStripMenuItem mRevert = new ToolStripMenuItem("Revert", null, menuRevert_Click);
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

            if (App.Repos.current == null)
                mNew.Enabled = false;

            if (!(tag is ClassCommit && !(tag as ClassCommit).isDefault))
                mEdit.Enabled = false;

            if (!(tag is ClassCommit && (tag as ClassCommit).files.Count == 0 && !(tag as ClassCommit).isDefault))
                mDel.Enabled = false;

            return menu;
        }

        /// <summary>
        /// New commit bundle needed
        /// </summary>
        private void menuNewCommit_Click(object sender, EventArgs e)
        {
            FormCommit commitForm = new FormCommit(false);
            commitForm.SetFiles(repo.commits.bundle[0]);
            if (commitForm.ShowDialog() == DialogResult.OK)
            {
                // Create a new commit bundle and move selected files to it
                repo.commits.NewBundle(commitForm.GetDescription(), commitForm.GetFiles());
                commitsRefresh();
            }
        }

        /// <summary>
        /// Edit selected commit bundle
        /// </summary>
        private void menuEditCommit_Click(object sender, EventArgs e)
        {
            // Recover the commit class bundle that was selected
            ClassCommit c = (sender as ToolStripMenuItem).Tag as ClassCommit;

            FormCommit commitForm = new FormCommit(false, c.description);
            commitForm.SetFiles(c);
            if (commitForm.ShowDialog() == DialogResult.OK)
            {
                // Update the description text and the list of files
                c.description = commitForm.GetDescription();

                // Renew only files that were left checked, the rest add back to the Default commit
                List<string> removedFiles = c.Renew(commitForm.GetFiles());
                repo.commits.bundle[0].AddFiles(removedFiles);
                commitsRefresh();
            }
        }

        /// <summary>
        /// Submit selected files within a changelist
        /// </summary>
        private void menuSubmit_Click(object sender, EventArgs e)
        {
            // If the right-click selected a changelist bundle, submit it
            // All the files that were selected should be checked in the list
            ClassCommit c = null;
            if ((sender as ToolStripMenuItem).Tag is ClassCommit)
                c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            else
            {
                // If the right-click selected files, gather all files selected
                // into a pseudo-bundle to submit
                c = new ClassCommit("ad-hoc");
                List<string> files = new List<string>();
                foreach (var n in treeCommits.SelectedNodes)
                    if (Status.isMarked(n.Tag.ToString()))
                        files.Add(n.Tag.ToString());
                c.AddFiles(files);
            }

            FormCommit commitForm = new FormCommit(true, c.description);
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
                        (commitForm.GetCheckAmend()==true? " --amend -- " : " -- ") +
                        String.Join(" ", final.Select(s => "\"" + s + "\"").ToList());

                    App.Git.Run(cmd);

                    File.Delete(tempFile);

                    // If the current commit bundle is not default, remove it. Refresh which follows
                    // will reset all files which were _not_ submitted as part of this change to be
                    // moved to the default changelist.
                    if (!c.isDefault)
                    {
                        App.Repos.current.commits.bundle.Remove(c);
                    }
                }
                App.Refresh();
            }
        }

        /// <summary>
        /// Delete empty commit bundle.
        /// It is already verified that the commit bundle (sent in the tag) is empty.
        /// </summary>
        private void menuDeleteEmpty_Click(object sender, EventArgs e)
        {
            // Recover the commit class bundle that was selected
            ClassCommit c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            repo.commits.bundle.Remove(c);
            commitsRefresh();
        }

        /// <summary>
        /// Diff the selected file against the repo
        /// </summary>
        private void menuDiff_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Tag is string)
            {
                string cmd = "difftool --cached -- \"" + (sender as ToolStripMenuItem).Tag.ToString() + "\"";
                App.Git.Run(cmd);
                App.Refresh();
            }
        }

        /// <summary>
        /// Revert selected files or a submit bundle
        /// </summary>
        private void menuRevert_Click(object sender, EventArgs e)
        {
            // If the right-click selected a changelist bundle, revert it
            // Use the class commit as a helper class to hold a set of files
            ClassCommit c = null;
            if ((sender as ToolStripMenuItem).Tag is ClassCommit)
            {
                c = (sender as ToolStripMenuItem).Tag as ClassCommit;
            }
            else
            {
                // If the right-click selected files, gather all files selected
                // into a pseudo-bundle to submit
                c = new ClassCommit("ad-hoc");
                List<string> files = new List<string>();
                foreach (var n in treeCommits.SelectedNodes)
                    if (Status.isMarked(n.Tag.ToString()))
                        files.Add(n.Tag.ToString());
                c.AddFiles(files);
            }

            // Get the files checked for commit
            if (c.files.Count > 0)
            {
                if (MessageBox.Show("Revert will unstage all the selected files and will lose the changes.\rProceed with Revert?", "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    string cmd = "reset HEAD -- " +
                    String.Join(" ", c.files.Select(s => "\"" + s + "\"").ToList());
                }
            }
        }
    }
}
