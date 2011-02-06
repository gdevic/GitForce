using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MultiSelectTreeview
{
    /// <summary>
    /// This class originated at: http://www.codeproject.com/KB/tree/Multiselect_Treeview.aspx
    /// </summary>
	public class MultiSelectTreeview : TreeView
	{
        private bool _overrideShift;

        /// <summary>
        /// This is a kind of hack to select all nodes (at the current expand level)
        /// Use the function of SHIFT+PAGE UP and SHIFT+PAGE DOWN that actually select
        /// all nodes before, and after the current node. And we override SHIFT (hacky).
        /// </summary>
        public void SelectAll()
        {
            _overrideShift = true;

            // Nothing is selected in the tree, this isn't a good state
            // select the top node
            if (_mSelectedNode == null || _mSelectedNode == Nodes[0])
            {
                SelectAllNodes(TopNode);
            }

            if (_mSelectedNode.Parent == null)
            {
                // Select all of the root nodes up to this point 
                if (Nodes.Count > 0)
                {
                    SelectNode(Nodes[0]);
                }
            }
            else
            {
                // Select all of the nodes up to this point under this nodes parent
                SelectNode(_mSelectedNode.Parent.FirstNode);
            }

            if (_mSelectedNode.Parent == null)
            {
                // Select the last ROOT node in the tree
                if (Nodes.Count > 0)
                {
                    SelectNode(Nodes[Nodes.Count - 1]);
                }
            }
            else
            {
                // Select the last node in this branch
                SelectNode(_mSelectedNode.Parent.LastNode);
            }

            _overrideShift = false;
        }

		#region Selected Node(s) Properties

		private readonly List<TreeNode> _mSelectedNodes;		
		public List<TreeNode> SelectedNodes
		{
			get
			{
				return _mSelectedNodes;
			}
			set
			{
				ClearSelectedNodes();
				if( value != null )
				{
					foreach( TreeNode node in value )
					{
						ToggleNode( node, true );
					}
				}
			}
		}

		// Note we use the new keyword to Hide the native treeview's SelectedNode property.
		private TreeNode _mSelectedNode;
		public new TreeNode SelectedNode
		{
			get { return _mSelectedNode; }
			set
			{
				ClearSelectedNodes();
				if( value != null )
				{
					SelectNode( value );
				}
			}
		}

		#endregion

		public MultiSelectTreeview()
		{
			_mSelectedNodes = new List<TreeNode>();
			base.SelectedNode = null;
		}

		#region Overridden Events

		protected override void OnGotFocus( EventArgs e )
		{
			// Make sure at least one node has a selection
			// this way we can tab to the ctrl and use the 
			// keyboard to select nodes
			try
			{
				if( _mSelectedNode == null && TopNode != null )
				{
					ToggleNode( TopNode, true );
				}

				base.OnGotFocus( e );
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			// If the user clicks on a node that was not
			// previously selected, select it now.

			try
			{
				base.SelectedNode = null;

				TreeNode node = GetNodeAt( e.Location );
				if( node != null )
				{
					int leftBound = node.Bounds.X; // - 20; // Allow user to click on image
					int rightBound = node.Bounds.Right + 10; // Give a little extra room
					if( e.Location.X > leftBound && e.Location.X < rightBound )
					{
						if( ModifierKeys == Keys.None && ( _mSelectedNodes.Contains( node ) ) )
						{
							// Potential Drag Operation
							// Let Mouse Up do select
						}
						else
						{							
							SelectNode( node );
						}
					}
				}

				base.OnMouseDown( e );
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			// If the clicked on a node that WAS previously
			// selected then, reselect it now. This will clear
			// any other selected nodes. e.g. A B C D are selected
			// the user clicks on B, now A C & D are no longer selected.
			try
			{
				// Check to see if a node was clicked on 
				TreeNode node = GetNodeAt( e.Location );
				if( node != null )
				{
					if( ModifierKeys == Keys.None && _mSelectedNodes.Contains( node ) )
					{
						int leftBound = node.Bounds.X; // -20; // Allow user to click on image
						int rightBound = node.Bounds.Right + 10; // Give a little extra room
						if( e.Location.X > leftBound && e.Location.X < rightBound )
						{

							SelectNode( node );
						}
					}
				}

				base.OnMouseUp( e );
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnItemDrag( ItemDragEventArgs e )
		{
			// If the user drags a node and the node being dragged is NOT
			// selected, then clear the active selection, select the
			// node being dragged and drag it. Otherwise if the node being
			// dragged is selected, drag the entire selection.
			try
			{
				TreeNode node = e.Item as TreeNode;

				if( node != null )
				{
					if( !_mSelectedNodes.Contains( node ) )
					{
						SelectSingleNode( node );
						ToggleNode( node, true );
					}
				}

				base.OnItemDrag( e );
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnBeforeSelect( TreeViewCancelEventArgs e )
		{
			// Never allow base.SelectedNode to be set!
			try
			{
				base.SelectedNode = null;
				e.Cancel = true;

				base.OnBeforeSelect( e );
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnAfterSelect( TreeViewEventArgs e )
		{
			// Never allow base.SelectedNode to be set!
			try
			{
				base.OnAfterSelect( e );
				base.SelectedNode = null;
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
		}

		protected override void OnKeyDown( KeyEventArgs e )
		{
			// Handle all possible key strokes for the control.
			// including navigation, selection, etc.

			base.OnKeyDown( e );

			if( e.KeyCode == Keys.ShiftKey ) return;

			//this.BeginUpdate();
			bool bShift = ( ModifierKeys == Keys.Shift );

			try
			{
				// Nothing is selected in the tree, this isn't a good state
				// select the top node
				if( _mSelectedNode == null && TopNode != null )
				{
					ToggleNode( TopNode, true );
				}

				// Nothing is still selected in the tree, this isn't a good state, leave.
				if( _mSelectedNode == null ) return;

				switch (e.KeyCode)
				{
				    case Keys.Left:
				        if( _mSelectedNode.IsExpanded && _mSelectedNode.Nodes.Count > 0 )
				        {
				            // Collapse an expanded node that has children
				            _mSelectedNode.Collapse();
				        }
				        else if( _mSelectedNode.Parent != null )
				        {
				            // Node is already collapsed, try to select its parent.
				            SelectSingleNode( _mSelectedNode.Parent );
				        }
				        break;
				    case Keys.Right:
				        if( !_mSelectedNode.IsExpanded )
				        {
				            // Expand a collpased node's children
				            _mSelectedNode.Expand();
				        }
				        else
				        {
				            // Node was already expanded, select the first child
				            SelectSingleNode( _mSelectedNode.FirstNode );
				        }
				        break;
				    case Keys.Up:
				        if( _mSelectedNode.PrevVisibleNode != null )
				        {
				            SelectNode( _mSelectedNode.PrevVisibleNode );
				        }
				        break;
				    case Keys.Down:
				        if( _mSelectedNode.NextVisibleNode != null )
				        {
				            SelectNode( _mSelectedNode.NextVisibleNode );
				        }
				        break;
				    case Keys.Home:
				        if( bShift )
				        {
				            if( _mSelectedNode.Parent == null )
				            {
				                // Select all of the root nodes up to this point 
				                if( Nodes.Count > 0 )
				                {
				                    SelectNode( Nodes[0] );
				                }
				            }
				            else
				            {
				                // Select all of the nodes up to this point under this nodes parent
				                SelectNode( _mSelectedNode.Parent.FirstNode );
				            }
				        }
				        else
				        {
				            // Select this first node in the tree
				            if( Nodes.Count > 0 )
				            {
				                SelectSingleNode( Nodes[0] );
				            }
				        }
				        break;
				    case Keys.End:
				        if( bShift )
				        {
				            if( _mSelectedNode.Parent == null )
				            {
				                // Select the last ROOT node in the tree
				                if( Nodes.Count > 0 )
				                {
				                    SelectNode( Nodes[Nodes.Count - 1] );
				                }
				            }
				            else
				            {
				                // Select the last node in this branch
				                SelectNode( _mSelectedNode.Parent.LastNode );
				            }
				        }
				        else
				        {
				            if( Nodes.Count > 0 )
				            {
				                // Select the last node visible node in the tree.
				                // Don't expand branches incase the tree is virtual
				                TreeNode ndLast = Nodes[0].LastNode;
				                while( ndLast.IsExpanded && ( ndLast.LastNode != null ) )
				                {
				                    ndLast = ndLast.LastNode;
				                }
				                SelectSingleNode( ndLast );
				            }
				        }
				        break;
				    case Keys.PageUp:
				        {
				            // Select the highest node in the display
				            int nCount = VisibleCount;
				            TreeNode ndCurrent = _mSelectedNode;
				            while( ( nCount ) > 0 && ( ndCurrent.PrevVisibleNode != null ) )
				            {
				                ndCurrent = ndCurrent.PrevVisibleNode;
				                nCount--;
				            }
				            SelectSingleNode( ndCurrent );
				        }
				        break;
				    case Keys.PageDown:
				        {
				            // Select the lowest node in the display
				            int nCount = VisibleCount;
				            TreeNode ndCurrent = _mSelectedNode;
				            while( ( nCount ) > 0 && ( ndCurrent.NextVisibleNode != null ) )
				            {
				                ndCurrent = ndCurrent.NextVisibleNode;
				                nCount--;
				            }
				            SelectSingleNode( ndCurrent );
				        }
				        break;
				    default:
				        {
				            // Assume this is a search character a-z, A-Z, 0-9, etc.
				            // Select the first node after the current node that 
				            // starts with this character
				            string sSearch = ( (char) e.KeyValue ).ToString();

				            TreeNode ndCurrent = _mSelectedNode;
				            while( ( ndCurrent.NextVisibleNode != null ) )
				            {
				                ndCurrent = ndCurrent.NextVisibleNode;
				                if( ndCurrent.Text.StartsWith( sSearch ) )
				                {
				                    SelectSingleNode( ndCurrent );
				                    break;
				                }
				            }
				        }
				        break;
				}
			}
			catch( Exception ex )
			{
				HandleException( ex );
			}
			finally
			{
				EndUpdate();
			}
		}

		#endregion

		#region Helper Methods

		private void SelectNode( TreeNode node )
		{
			try
			{
				BeginUpdate();

				if( _mSelectedNode == null || (ModifierKeys == Keys.Control && !_overrideShift))
				{
					// Ctrl+Click selects an unselected node, or unselects a selected node.
					bool bIsSelected = _mSelectedNodes.Contains( node );
					ToggleNode( node, !bIsSelected );
				}
				else if( ModifierKeys == Keys.Shift || _overrideShift )
				{
					// Shift+Click selects nodes between the selected node and here.
					TreeNode ndStart = _mSelectedNode;
					TreeNode ndEnd = node;

					if( ndStart.Parent == ndEnd.Parent )
					{
						// Selected node and clicked node have same parent, easy case.
						if( ndStart.Index < ndEnd.Index )
						{							
							// If the selected node is beneath the clicked node walk down
							// selecting each Visible node until we reach the end.
							while( ndStart != ndEnd )
							{
								ndStart = ndStart.NextVisibleNode;
								if( ndStart == null ) break;
								ToggleNode( ndStart, true );
							}
						}
						else if( ndStart.Index == ndEnd.Index )
						{
							// Clicked same node, do nothing
						}
						else
						{
							// If the selected node is above the clicked node walk up
							// selecting each Visible node until we reach the end.
							while( ndStart != ndEnd )
							{
								ndStart = ndStart.PrevVisibleNode;
								if( ndStart == null ) break;
								ToggleNode( ndStart, true );
							}
						}
					}
					else
					{
						// Selected node and clicked node have same parent, hard case.
						// We need to find a common parent to determine if we need
						// to walk down selecting, or walk up selecting.

						TreeNode ndStartP = ndStart;
						TreeNode ndEndP = ndEnd;
						int startDepth = Math.Min( ndStartP.Level, ndEndP.Level );

						// Bring lower node up to common depth
						while( ndStartP.Level > startDepth )
						{
							ndStartP = ndStartP.Parent;
						}

						// Bring lower node up to common depth
						while( ndEndP.Level > startDepth )
						{
							ndEndP = ndEndP.Parent;
						}

						// Walk up the tree until we find the common parent
						while( ndStartP.Parent != ndEndP.Parent )
						{
							ndStartP = ndStartP.Parent;
							ndEndP = ndEndP.Parent;
						}

						// Select the node
						if( ndStartP.Index < ndEndP.Index )
						{
							// If the selected node is beneath the clicked node walk down
							// selecting each Visible node until we reach the end.
							while( ndStart != ndEnd )
							{
								ndStart = ndStart.NextVisibleNode;
								if( ndStart == null ) break;
								ToggleNode( ndStart, true );
							}
						}
						else if( ndStartP.Index == ndEndP.Index )
						{
							if( ndStart.Level < ndEnd.Level )
							{
								while( ndStart != ndEnd )
								{
									ndStart = ndStart.NextVisibleNode;
									if( ndStart == null ) break;
									ToggleNode( ndStart, true );
								}
							}
							else
							{
								while( ndStart != ndEnd )
								{
									ndStart = ndStart.PrevVisibleNode;
									if( ndStart == null ) break;
									ToggleNode( ndStart, true );
								}
							}
						}
						else
						{
							// If the selected node is above the clicked node walk up
							// selecting each Visible node until we reach the end.
							while( ndStart != ndEnd )
							{
								ndStart = ndStart.PrevVisibleNode;
								if( ndStart == null ) break;
								ToggleNode( ndStart, true );
							}
						}
					}
				}
				else
				{
					// Just clicked a node, select it
					SelectSingleNode( node );
				}

				OnAfterSelect( new TreeViewEventArgs( _mSelectedNode ) );
			}
			finally
			{
				EndUpdate();
			}
		}

		private void ClearSelectedNodes()
		{
			try
			{
				foreach( TreeNode node in _mSelectedNodes )
				{
					node.BackColor = BackColor;
					node.ForeColor = ForeColor;
				}
			}
			finally
			{
				_mSelectedNodes.Clear();
				_mSelectedNode = null;
			}
		}

		private void SelectSingleNode( TreeNode node )
		{
			if( node == null )
			{
				return;
			}

			ClearSelectedNodes();
			ToggleNode( node, true );
			node.EnsureVisible();
		}

        private void SelectAllNodes(TreeNode root)
        {
            foreach (TreeNode tn in root.Nodes)
                SelectAllNodes(tn);
            ToggleNode(root, true);
        }

		private void ToggleNode( TreeNode node, bool bSelectNode )
		{
			if( bSelectNode )
			{
				_mSelectedNode = node;
				if( !_mSelectedNodes.Contains( node ) )
				{
					_mSelectedNodes.Add( node );
				}
				node.BackColor = SystemColors.Highlight;
				node.ForeColor = SystemColors.HighlightText;
			}
			else
			{
				_mSelectedNodes.Remove( node );
				node.BackColor = BackColor;
				node.ForeColor = ForeColor;
			}
		}

		private void HandleException( Exception ex )
		{
			// Perform some error handling here.
			// We don't want to bubble errors to the CLR. 
			MessageBox.Show( ex.Message );
		}

		#endregion
	}
}
