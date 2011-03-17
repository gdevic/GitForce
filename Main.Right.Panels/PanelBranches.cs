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

        /// <summary>
        /// Class containing sets of branches for the current repo
        /// </summary>
        private readonly ClassBranches _branches = new ClassBranches();

        public PanelBranches()
        {
            InitializeComponent();

            App.Refresh += BranchesRefresh;
        }

        /// <summary>
        /// Refresh the tree view of branches
        /// </summary>
        private void BranchesRefresh()
        {
            _branches.Refresh();

            // MONO BUG: Treeview faults when expanding past the need to scroll in this particular case
            //            Workaround is to turn off scrollable while expaning and then turn them back on
            treeBranches.Scrollable = false;
            treeBranches.BeginUpdate();
            treeBranches.Nodes.Clear();

            // We always list local and remote branches nodes
            TreeNode tnLocal = new TreeNode("Local Branches");
            TreeNode tnRemote = new TreeNode("Remote Branches");

            // Add all local branches to the tree
            foreach (string s in _branches.Local)
            {
                TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                tn.Tag = s;
                if (s == _branches.Current)
                    tn.SelectedImageIndex = tn.ImageIndex = (int)BranchIcons.BranchSelected;
                tnLocal.Nodes.Add(tn);
            }

            // Add all remote branches to the tree
            foreach (string s in _branches.Remote)
            {
                TreeNode tn = new TreeNode(s, (int)BranchIcons.BranchIdle, (int)BranchIcons.BranchIdle);
                tn.Tag = s;
                tnRemote.Nodes.Add(tn);
            }

            treeBranches.Nodes.Add(tnLocal);
            treeBranches.Nodes.Add(tnRemote);
            treeBranches.ExpandAll();

            treeBranches.EndUpdate();
            treeBranches.Scrollable = true;
        }

        /// <summary>
        /// Return the name of a current branch
        /// </summary>
        public string GetCurrent()
        {
            return _branches.Current;
        }

        /// <summary>
        /// Switch to a new branch name
        /// </summary>
        public void SetCurrent(string name)
        {
            if (_branches.SwitchTo(name))
                App.Refresh();
        }

        /// <summary>
        /// Helper function that returns selected branch name in the tree view or null
        /// if no valid branch node was selected
        /// </summary>
        private string GetSelectedNode()
        {
            if (treeBranches.SelectedNode != null && treeBranches.SelectedNode.Tag != null)
                return treeBranches.SelectedNode.Tag.ToString();
            else
                return null;
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void TreeBranchesMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Build the context menu to be shown
                treeBranches.SelectedNode = treeBranches.GetNodeAt(e.X, e.Y);
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, GetSelectedNode()));
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for branches
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, string selected)
        {
            string sel = GetSelectedNode();

            // Menus are set in this order:
            //  [0]  New...        -> always open a dialog
            //  [1]  Delete...     -> always open a dialog
            //  [2]  Switch to     -> dialog or switch: local branches only, different from current
            //  [3]  Merge with    -> dialog or merge: branch different from current

            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, MenuNewBranchClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, MenuDeleteBranchClick);

            ToolStripMenuItem mSwitchTo;
            if (sel != _branches.Current && _branches.Local.IndexOf(sel) >= 0)
                mSwitchTo = new ToolStripMenuItem("Switch to " + sel, null, TreeBranchesDoubleClick);
            else
                mSwitchTo = new ToolStripMenuItem("Switch to...", null, MenuSwitchClick);

            ToolStripMenuItem mMergeWith;
            if (sel != _branches.Current && (_branches.Local.IndexOf(sel) >= 0 || _branches.Remote.IndexOf(sel) >= 0))
                mMergeWith = new ToolStripMenuItem("Merge with " + sel, null, MenuMergeClick);
            else
                mMergeWith = new ToolStripMenuItem("Merge with...", null, MenuMergeClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mDelete, mSwitchTo, mMergeWith
            });

            if (sel == null)
                mDelete.Enabled = mSwitchTo.Enabled = mMergeWith.Enabled = false;

            mNew.Enabled = App.Repos.Current != null;

            return menu;
        }

        /// <summary>
        /// Create a new branch using a dialog to select the branch
        /// </summary>
        private static void MenuNewBranchClick(object sender, EventArgs e)
        {
            FormNewBranch newBranch = new FormNewBranch();
            if (newBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Delete a branch using a dialog to select the branch
        /// </summary>
        private static void MenuDeleteBranchClick(object sender, EventArgs e)
        {
            FormDeleteBranch delBranch = new FormDeleteBranch();
            if (delBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Switch to a branch using a dialog to select the branch
        /// </summary>
        private static void MenuSwitchClick(object sender, EventArgs e)
        {
            FormSwitchToBranch switchBranch = new FormSwitchToBranch();
            if (switchBranch.ShowDialog() == DialogResult.OK)
                App.Refresh();
        }

        /// <summary>
        /// Double-clicking on a local branch name will switch to it
        /// This is also a handler to switch to a selected branch using context menu
        /// </summary>
        private void TreeBranchesDoubleClick(object sender, EventArgs e)
        {
            SetCurrent(GetSelectedNode());
        }

        /// <summary>
        /// Merge branch menu item
        /// </summary>
        private void MenuMergeClick(object sender, EventArgs e)
        {
            FormMergeBranch mergeBranch = new FormMergeBranch(_branches);
            if (mergeBranch.ShowDialog() == DialogResult.OK)
            {
                string cmd = mergeBranch.GetCmd();
                App.Repos.Current.Run(cmd);
                App.Refresh();
            }
        }
    }
}
