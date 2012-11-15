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
    public partial class PanelBranches : UserControl
    {
        enum BranchIcons { Branch, BranchIdle, BranchSelected };

        public PanelBranches()
        {
            InitializeComponent();

            App.Refresh += BranchesRefresh;
        }

        /// <summary>
        /// Refresh the tree view of branches
        /// </summary>
        public void BranchesRefresh()
        {
            // Use predefined [0] for local and [1] for remote branches
            treeBranches.Nodes[0].Nodes.Clear();
            treeBranches.Nodes[1].Nodes.Clear();

            if (App.Repos.Current != null)
            {
                ClassBranches branches = App.Repos.Current.Branches;
                branches.Refresh();

                // Add all local branches to the tree
                foreach (string s in branches.Local)
                {
                    TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                    tn.Tag = s;
                    if (s == branches.Current)
                        tn.SelectedImageIndex = tn.ImageIndex = (int)BranchIcons.BranchSelected;
                    treeBranches.Nodes[0].Nodes.Add(tn);
                }

                // Add all remote branches to the tree
                foreach (string s in branches.Remote)
                {
                    TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                    tn.Tag = s;
                    treeBranches.Nodes[1].Nodes.Add(tn);
                }
            }
            // By default, expand branches
            treeBranches.ExpandAll();
        }

        /// <summary>
        /// Helper function that returns selected branch name in the tree view or null
        /// if no valid branch node was selected
        /// </summary>
        private string GetSelectedBranch()
        {
            if (treeBranches.SelectedNode != null && treeBranches.SelectedNode.Tag != null)
                return treeBranches.SelectedNode.Tag.ToString();
            return null;
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void TreeBranchesMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && App.Repos.Current != null)
            {
                // Build the context menu to be shown
                treeBranches.SelectedNode = treeBranches.GetNodeAt(e.X, e.Y);
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for branches
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            // Menus are set in this order:
            //  [0]  New...        -> always open a dialog
            //  [1]  Delete...     -> always open a dialog
            //  [2]  Switch to     -> dialog or switch: local branches only, different from current
            //  [3]  Merge with    -> dialog or merge: branch different from current

            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, MenuNewBranchClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, MenuDeleteBranchClick);
            ToolStripMenuItem mSwitchTo = new ToolStripMenuItem("Switch to...", null, MenuSwitchClick);
            ToolStripMenuItem mMergeWith = new ToolStripMenuItem("Merge with...", null, MenuMergeClick);

            if (App.Repos.Current != null)
            {
                ClassBranches branches = App.Repos.Current.Branches;
                string sel = GetSelectedBranch();

                if (sel != null)
                {
                    if (sel != branches.Current && branches.Local.IndexOf(sel) >= 0)
                        mSwitchTo = new ToolStripMenuItem("Switch to " + sel, null, TreeBranchesDoubleClick);

                    if (sel != branches.Current &&
                        (branches.Local.IndexOf(sel) >= 0 || branches.Remote.IndexOf(sel) >= 0))
                        mMergeWith = new ToolStripMenuItem("Merge with " + sel, null, MenuMergeClick);
                }
                else
                    mDelete.Enabled = mSwitchTo.Enabled = mMergeWith.Enabled = false;
            }
            else
                mNew.Enabled = mDelete.Enabled = mSwitchTo.Enabled = mMergeWith.Enabled = false;

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mDelete, mSwitchTo, mMergeWith });

            return menu;
        }

        #region Operations on a branch

        /// <summary>
        /// Double-clicking on a local branch name will switch to it.
        /// Also a context menu handler.
        /// </summary>
        private void TreeBranchesDoubleClick(object sender, EventArgs e)
        {
            App.Repos.Current.Branches.SwitchTo(GetSelectedBranch());
            App.DoRefresh();
        }

        /// <summary>
        /// Switch to a branch using a dialog to select the branch
        /// </summary>
        private static void MenuSwitchClick(object sender, EventArgs e)
        {
            FormSwitchToBranch switchBranch = new FormSwitchToBranch();
            if (switchBranch.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        /// <summary>
        /// Create a new branch using a dialog to select the branch
        /// </summary>
        private static void MenuNewBranchClick(object sender, EventArgs e)
        {
            FormNewBranch newBranch = new FormNewBranch();
            if (newBranch.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        /// <summary>
        /// Delete a branch using a dialog to select the branch
        /// </summary>
        private static void MenuDeleteBranchClick(object sender, EventArgs e)
        {
            FormDeleteBranch delBranch = new FormDeleteBranch();
            if (delBranch.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        /// <summary>
        /// Merge branch using a dialog to select the branch
        /// </summary>
        private void MenuMergeClick(object sender, EventArgs e)
        {
            FormMergeBranch mergeBranch = new FormMergeBranch();
            if (mergeBranch.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        #endregion
    }
}
