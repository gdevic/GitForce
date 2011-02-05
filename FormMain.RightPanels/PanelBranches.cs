using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormMain_RightPanels
{
    public partial class PanelBranches : UserControl
    {
        enum BranchIcons { Branch, BranchIdle, BranchSelected };

        /// <summary>
        /// Class containing sets of branches for the current repo
        /// </summary>
        private ClassBranches branches = new ClassBranches();

        public PanelBranches()
        {
            InitializeComponent();

            App.Refresh += branchesRefresh;
        }

        /// <summary>
        /// Refresh the tree view of branches
        /// </summary>
        private void branchesRefresh()
        {
            branches.Refresh();

            treeBranches.BeginUpdate();
            treeBranches.Nodes.Clear();

            // We always list local and remote branches nodes
            TreeNode tnLocal = new TreeNode("Local Branches");
            TreeNode tnRemote = new TreeNode("Remote Branches");

            // Add all local branches to the tree
            foreach (string s in branches.local)
            {
                TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                tn.Tag = s;
                if (s == branches.current)
                    tn.SelectedImageIndex = tn.ImageIndex = (int)BranchIcons.BranchSelected;
                tnLocal.Nodes.Add(tn);
            }

            // Add all remote branches to the tree
            foreach (string s in branches.remote)
            {
                TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                tn.Tag = s;
                tnRemote.Nodes.Add(tn);
            }

            treeBranches.Nodes.Add(tnLocal);
            treeBranches.Nodes.Add(tnRemote);
            treeBranches.ExpandAll();

            treeBranches.EndUpdate();
        }

        /// <summary>
        /// Return the name of a current branch
        /// </summary>
        public string GetCurrent() 
        {
            return branches.current;
        }

        /// <summary>
        /// Switch to a new branch name
        /// </summary>
        public void SetCurrent(string name)
        {
            if (branches.SwitchTo(name))
                App.Refresh();
        }

        /// <summary>
        /// Helper function that returns selected branch name in the tree view or null
        /// if no valid branch node was selected
        /// </summary>
        private string getSelectedNode()
        {
            if (treeBranches.SelectedNode != null && treeBranches.SelectedNode.Tag != null)
                return treeBranches.SelectedNode.Tag.ToString();
            else
                return null;
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void treeBranches_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Build the context menu to be shown
                treeBranches.SelectedNode = treeBranches.GetNodeAt(e.X, e.Y);
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, getSelectedNode()));
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for branches
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, string selected)
        {
            string sel = getSelectedNode();

            // Menus are set in this order:
            //  [0]  New...        -> always open a dialog
            //  [1]  Delete...     -> always open a dialog
            //  [2]  Switch to     -> dialog or switch: local branches only, different from current
            //  [3]  Merge with    -> dialog or merge: branch different from current

            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, menuNewBranch_Click);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, menuDeleteBranch_Click);

            ToolStripMenuItem mSwitchTo;
            if (sel != branches.current && branches.local.IndexOf(sel) >= 0)
                mSwitchTo = new ToolStripMenuItem("Switch to " + sel, null, treeBranches_DoubleClick);
            else
                mSwitchTo = new ToolStripMenuItem("Switch to...", null, menuSwitch_Click);

            ToolStripMenuItem mMergeWith;
            if (sel != branches.current && (branches.local.IndexOf(sel) >= 0 || branches.remote.IndexOf(sel) >= 0))
                mMergeWith = new ToolStripMenuItem("Merge with " + sel, null, menuMerge_Click);
            else
                mMergeWith = new ToolStripMenuItem("Merge with...", null, menuMerge_Click);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mDelete, mSwitchTo, mMergeWith
            });

            if (sel == null)
                mDelete.Enabled = mSwitchTo.Enabled = mMergeWith.Enabled = false;

            mNew.Enabled = App.Repos.current != null;

            return menu;
        }

        /// <summary>
        /// Create a new branch using a dialog to select the branch
        /// </summary>
        private void menuNewBranch_Click(object sender, EventArgs e)
        {
            FormNewBranch newBranch = new FormNewBranch();
            if (newBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Delete a branch using a dialog to select the branch
        /// </summary>
        private void menuDeleteBranch_Click(object sender, EventArgs e)
        {
            FormDeleteBranch delBranch = new FormDeleteBranch();
            if (delBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Switch to a branch using a dialog to select the branch
        /// </summary>
        private void menuSwitch_Click(object sender, EventArgs e)
        {
            FormSwitchToBranch switchBranch = new FormSwitchToBranch();
            if (switchBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Double-clicking on a local branch name will switch to it
        /// This is also a handler to switch to a selected branch using context menu
        /// </summary>
        private void treeBranches_DoubleClick(object sender, EventArgs e)
        {
            SetCurrent(getSelectedNode());
        }

        private void menuMerge_Click(object sender, EventArgs e)
        {
            FormMergeBranch mergeBranch = new FormMergeBranch(branches);
            if (mergeBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }
    }
}
