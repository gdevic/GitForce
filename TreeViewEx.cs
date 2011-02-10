using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace git4win
{
    /// <summary>
    /// Yet another TreeViewEx control, written to be suitable for this project.
    /// 
    /// Adds to the standard TreeView the following features:
    /// * Multi-selection via new property SelectedNodes[]
    /// * Deep selection: recursive selection into children nodes
    /// 
    /// Important: Instead of "nodes.Clear()", call this NodesClear() function!
    /// </summary>
	public class TreeViewEx : TreeView
    {
        /// <summary>
        /// Private list of selected nodes
        /// </summary>
        private readonly List<TreeNode> _selectedNodes;

        /// <summary>
        /// Public property of the TreeViewEx to return or set a list of selected nodes
        /// </summary>
        public List<TreeNode> SelectedNodes
        {
            get { return _selectedNodes; }
            set { SetSelectedNodes(value); }
        }

        /// <summary>
        /// Return a single selected node
        /// </summary>
        public new TreeNode SelectedNode { get; set; }

        /// <summary>
        /// TreeViewEx constructor
        /// </summary>
        public TreeViewEx()
        {
            _selectedNodes = new List<TreeNode>();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode node = GetNodeAt(e.Location);
            if( node!=null )
            {
                this.BeginUpdate();
                if (ModifierKeys == Keys.None)
                {
                    if (!_selectedNodes.Contains(node))
                    {
                        SelectNone();
                        SetSelected(node, true);
                        SelectedNode = node;
                    }
                }
                if(ModifierKeys==Keys.Control)
                {
                    SetSelectedDeep(node, !_selectedNodes.Contains(node));
                    SelectedNode = node;
                }
                if(ModifierKeys==Keys.Shift)
                {
                    // Select all nodes from the last selected one to the current one
                    // Under the same parent, recursively into their children nodes
                    if(node.Parent!=null)
                    {
                        List<TreeNode> siblings = node.Parent.Nodes.Cast<TreeNode>().ToList();
                        if(siblings.Contains(SelectedNode))
                        {
                            int isel = siblings.IndexOf(SelectedNode);
                            int icur = siblings.IndexOf(node);
                            int istart = Math.Min(isel, icur);
                            int iend = istart + Math.Abs(isel - icur) + 1;
                            for (int i = istart; i < iend; i++)
                                SetSelected(siblings[i], true);
                        }
                    }
                }
                this.EndUpdate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            TreeNode node = GetNodeAt(e.Location);
            if(node!=null)
            {
                this.BeginUpdate();
                if (ModifierKeys == Keys.None && e.Button==MouseButtons.Left)
                {
                    SelectNone();
                    SetSelected(node, true);
                    SelectedNode = node;
                }
                this.EndUpdate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.SelectedNode = null;
            e.Cancel = true;
            base.OnBeforeSelect(e);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            var node = e.Item as TreeNode;
            if(node !=null)
            {
                if (!_selectedNodes.Contains(node))
                {
                    SelectNone();
                    SetSelected(node, true);
                }
            }
            base.OnItemDrag(e);
        }

        /// <summary>
        /// Select or deselect a given node
        /// </summary>
        private void SetSelected(TreeNode node, bool bSelect)
        {
            if( bSelect)
            {
                if (!_selectedNodes.Contains(node))
                    _selectedNodes.Add(node);

                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                _selectedNodes.Remove(node);
                node.BackColor = BackColor;
                node.ForeColor = ForeColor;
            }
        }

        /// <summary>
        /// Deep selection sets the given node and all of its children, recursively.
        /// </summary>
        private void SetSelectedDeep(TreeNode node, bool bSelect)
        {
            foreach (TreeNode n in node.Nodes)
                SetSelectedDeep(n, bSelect);
            SetSelected(node, bSelect);
        }

        /// <summary>
        /// Select every node in the tree
        /// </summary>
        public void SelectAll()
        {
            this.BeginUpdate();
            foreach (TreeNode node in this.Nodes)
                SetSelectedDeep(node, true);
            this.EndUpdate();
        }

        /// <summary>
        /// Deselect all nodes in the tree
        /// </summary>
        public void SelectNone()
        {
            this.BeginUpdate();
            foreach (TreeNode node in this.Nodes)
                SetSelectedDeep(node, false);
            this.EndUpdate();
        }

        /// <summary>
        /// Clear the tree from all the nodes. Use this call instead of treeView.Nodes.Clear()
        /// </summary>
        public void NodesClear()
        {
            _selectedNodes.Clear();
            this.Nodes.Clear();
        }

        /// <summary>
        /// Alternate way to select specific nodes by sending it a list
        /// </summary>
        public void SetSelectedNodes(List<TreeNode> nodes)
        {
            SelectNone();
            this.BeginUpdate();
            foreach (TreeNode n in nodes)
            {
                SetSelected(n, true);
                n.EnsureVisible();
            }
            this.EndUpdate();
        }
    }
}
