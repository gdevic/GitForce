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
using System.Diagnostics;

namespace git4win.FormMain_LeftPanels
{
    public partial class PanelView : UserControl
    {
        /// <summary>
        /// Current format can be tree view or list view
        /// </summary>
        private bool isTreeView = true;

        /// <summary>
        /// Status class containing the git status of files
        /// </summary>
        private ClassStatus Status = null;

        /// <summary>
        /// Structure holding the tree selection items information
        /// </summary>
        private struct TSelection
        {
            public string selPath;
            public string[] selFiles;
            public string tag;

            /// <summary>
            /// Returns the list of files (from selFiles) formatted for git file list command --
            /// Absolute paths are shortened into paths relative to the current repo root,
            /// Quotes are added around each file path name,
            /// All files are joined into one resulting string
            /// </summary>
            public string selFilesGitFormat()
            {
                return String.Join(" ", selFiles.
                    Select(s => "\"" + s.Substring(App.Repos.current.root.Length + 1) + "\"").ToList());
            }

            /// <summary>
            /// Returns the selPath file formatted for git file list command --
            /// Absolute path is shortened into path relative to the current repo root,
            /// Quotes are added around the file path name
            /// </summary>
            /// <returns></returns>
            public string selPathGitFormat()
            {
                return "\"" + selPath.Substring(App.Repos.current.root.Length + 1) + "\"";
            }
        };

        public PanelView()
        {
            InitializeComponent();

            treeView.ImageList = ClassStatus.GetImageList();

            App.Refresh += viewRefresh;

            // Initialize the current view
            // Current view mode is persistent and stored in in Properties.Settings.Default.viewMode (int)
            SetView(Properties.Settings.Default.viewMode);
        }

        /// <summary>
        /// Set the view mode by sending a menu item whose Tag contains the mode number.
        /// This function is called from a menu handlers that select view mode.
        /// </summary>
        public void viewSetByMenuItem(object sender, EventArgs e)
        {
            SetView(int.Parse((sender as ToolStripMenuItem).Tag.ToString()));
        }

        /// <summary>
        /// Sets the current view mode
        /// </summary>
        public void SetView(int mode)
        {
            // Set the menu bullet to the current view
            List<ToolStripMenuItem> viewMenus = new List<ToolStripMenuItem> {
                menuView0, menuView1, menuView2, menuView3, menuView4 };
            foreach (var m in viewMenus)
                m.Checked = false;
            viewMenus[mode].Checked = true;

            Properties.Settings.Default.viewMode = mode;
            viewRefresh();
        }

        /// <summary>
        /// Panel view class refresh function
        /// </summary>
        public void viewRefresh()
        {
            int mode = Properties.Settings.Default.viewMode;

            // Define root initial icons for 5 viewing modes
            int[] icons = { (int)ClassStatus.Img.FOLDER_OPENED, 
                            (int)ClassStatus.Img.FOLDER_OPENED, 
                            (int)ClassStatus.Img.DATABASE_OPENED,
                            (int)ClassStatus.Img.FOLDER_OPENED,
                            (int)ClassStatus.Img.FOLDER_OPENED };

            // Set the view mode text (picked up from the indexed menu combo box)
            viewLabel.Text = dropViewMode.DropDownItems[mode].Text;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            if (App.Repos.current != null)
            {
                Status = new ClassStatus(App.Repos.current);

                switch (mode)
                {
                    case 0:     // Git view of local files: status + untracked
                        Status.SetListByCommand("status --porcelain -uall -z *");
                        Status.Seal();
                        break;
                    case 1:     // Git view of files: status - untracked
                        Status.SetListByCommand("status --porcelain -uno -z *");
                        Status.Filter(s => s.StartsWith("??"));
                        Status.Seal();
                        break;
                    case 2:     // Git view of repo: ls-tree
                        Status.SetListByCommand("ls-tree --abbrev -r -z HEAD");
                        Status.ConvertSeal();
                        break;
                    case 3:     // Local file view: use local directory list
                        Status.SetListByCommand("status --porcelain -uall -z *");
                        Status.Seal();
                        Status.LoadLocalFiles();
                        Status.Seal();
                        break;
                    case 4:     // Local files not in repo: untracked only
                        Status.SetListByCommand("status --porcelain -uall -z *");
                        Status.Filter(s => !s.StartsWith("??"));
                        Status.Seal();
                        break;
                }

                // Set the file sorting order
                if (Properties.Settings.Default.sortByExtension)
                    Status.sortBy = GitDirectoryInfo.SortBy.Extension;
                else
                    Status.sortBy = GitDirectoryInfo.SortBy.Name;

                // Build the tree view (or a list view)
                TreeNode node = Status.BuildView(isTreeView);

                // Add the resulting tree to the tree view control
                treeView.Nodes.Add(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = icons[mode];

                // Assign the icons to the nodes of tree view
                Status.viewAssignIcon(node, false);

                // Always keep the root node expanded by default
                node.Expand();

                // Finally, expand the rest of the tree to its previous expand state
                viewExpand(node);

            }
            treeView.EndUpdate();
        }

        /// <summary>
        /// Refresh the tree view pane
        /// </summary>
        private void menuRefresh(object sender, EventArgs e)
        {
            viewRefresh();
        }

        /// <summary>
        /// Traverse tree and expand those nodes which are marked as expanded
        /// </summary>
        private void viewExpand(TreeNode rootNode)
        {
            TreeNodeCollection nodes = rootNode.Nodes;
            foreach (TreeNode tn in nodes)
            {
                if( App.Repos.current.isExpanded(tn.Tag.ToString()))
                {
                    tn.Expand();
                    tn.ImageIndex |= 1;
                }
                viewExpand(tn);
            }
        }

        /// <summary>
        /// Callback called when user clicks on a node to expand it
        /// </summary>
        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            App.Repos.current.ExpansionSet(tn.Tag.ToString());
            tn.ImageIndex |= 1;
        }

        /// <summary>
        /// Callback called when user clicks on a node to collapse it
        /// </summary>
        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            // If the user closed the root node, collapse everything
            if (tn == treeView.Nodes[0])
            {
                App.Repos.current.ExpansionReset(null);
                treeView.CollapseAll();
            }
            else
                App.Repos.current.ExpansionReset(tn.Tag.ToString());
            tn.ImageIndex &= ~1;
        }

        /// <summary>
        /// Handle double-clicking on a tree view
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode sel = treeView.SelectedNode;
            if (sel != null && !sel.Tag.ToString().EndsWith(@"\"))
            {
                // Perform the required action on double-click
                string option = Properties.Settings.Default.DoubleClick;
                string program = Properties.Settings.Default.DoubleClickProgram;

                if (option == "1")
                    Process.Start(sel.Tag.ToString());
                if (option == "2")
                    Process.Start(program, sel.Tag.ToString());
                viewRefresh();
            }
        }

        /// <summary>
        /// Drag and drop handler. User selected one or more files and started to drag them (away).
        /// Send a set of files through their full path name.
        /// </summary>
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            string[] files = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();
            DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Move);
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// As the mouse moves over nodes, show the human readable description of
        /// files that the mouse points to
        /// </summary>
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (Status != null)
                Status.ShowTreeInfo(treeView.GetNodeAt(e.X, e.Y));
        }

        /// <summary>
        /// Toggle between list view and tree view
        /// </summary>
        private void btListView_Click(object sender, EventArgs e)
        {
            btListView.Checked = isTreeView;
            isTreeView = !isTreeView;
            viewRefresh();
        }

        /// <summary>
        /// Track the checked setting and refresh the local view when changed
        /// </summary>
        private void menuSortFilesByExtension_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.sortByExtension = menuSortFilesByExtension.Checked;
            viewRefresh();
        }

        /// <summary>
        /// Select All function handler: select all files
        /// </summary>
        public void SelectAll()
        {
            // TOOD: SelectAll should be grayed out to start with if there are no nodes
            if (treeView.Nodes.Count > 0)
                treeView.SelectAll();
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Build the context menu to be shown
                TreeNode sel = treeView.GetNodeAt(e.X, e.Y) ?? treeView.Nodes[0];

                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu, sel.Tag as string));
            }
        }

        /// <summary>
        /// Helper function to create a menu item
        /// </summary>
        private ToolStripMenuItem CreateMenu(string name, EventHandler onClick, TSelection sel, string tag="")
        {
            ToolStripMenuItem menu = new ToolStripMenuItem(name, null, onClick);
            sel.tag = tag;
            menu.Tag = sel;
            return menu;
        }

        /// <summary>
        /// Builds ands returns a context menu for files
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner, string path)
        {
            // If there is no current repo, nothing to build
            if (App.Repos.current == null)
                return new ToolStripItemCollection(owner, new ToolStripItem[] {} );

            TSelection sel = new TSelection();
            sel.selFiles = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();

            // Empty path is only sent from the main menu (not the context right-click handler)
            // in which case consider selected file first file in the selected list
            if (string.IsNullOrEmpty(path))
                sel.selPath = sel.selFiles.Count() > 0? 
                    sel.selPath = sel.selFiles[0] :
                    App.Repos.current.root;
            else
                sel.selPath = path;

            App.Execute.Add(sel.selPath + Environment.NewLine);

            ToolStripMenuItem mUpdate = CreateMenu("Update Changelist", menuViewUpdateChangelist_Click, sel);
            ToolStripMenuItem mRevert = CreateMenu("Revert", menuViewRevert_Click, sel);


            // Build the "Diff vs" submenu
            ToolStripMenuItem mDiffIndex = CreateMenu("Index", menuViewDiff_Click, sel);
            ToolStripMenuItem mDiffHead = CreateMenu("Repository HEAD", menuViewDiff_Click, sel, "HEAD");
            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs");
            mDiff.DropDownItems.Add(mDiffIndex);
            mDiff.DropDownItems.Add(mDiffHead);

            // Build the "Edit Using" submenus
            // The default option is to open the file using the OS-associated editor,
            // after which all the user-specified programs are listed
            ToolStripMenuItem mEditAssoc = CreateMenu("Associated Editor", menuViewEdit_Click, sel);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Using");
            mEdit.DropDownItems.Add(mEditAssoc);
            string[] progs = Properties.Settings.Default.EditViewPrograms.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                mEdit.DropDownItems.Add(CreateMenu(Path.GetFileName(s), menuViewEdit_Click, sel, s));

            ToolStripMenuItem mExplore = CreateMenu("Explore...", menuViewExplore_Click, sel);
            ToolStripMenuItem mCommand = CreateMenu("Command Prompt...", menuViewCommand_Click, sel);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mUpdate,
                mRevert,
                mDiff,
                mEdit,
                new ToolStripSeparator(),
                mExplore, mCommand
            });

            if (sel.selPath.EndsWith(@"\"))
                mRevert.Enabled = 
                mDiff.Enabled =
                mEdit.Enabled = false;

            return menu;
        }

        /// <summary>
        /// Update changelist (index) with selected files
        /// </summary>
        private void menuViewUpdateChangelist_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)(sender as ToolStripDropDownItem).Tag;
            App.Repos.current.Run("add -- " + sel.selFilesGitFormat());
            App.Refresh();      // App-wide refresh since 'add' modifies other panes
        }

        /// <summary>
        /// Discard changes to selected files in the working set
        /// </summary>
        private void menuViewRevert_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)(sender as ToolStripDropDownItem).Tag;
            if (MessageBox.Show("This will revert all changes to selected files in your working directory. It will not affect staged files in Changelists.\r\rProceed with Revert?",
                "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                App.Repos.current.Run("checkout -- " + sel.selFilesGitFormat());
                viewRefresh();
            }
        }

        /// <summary>
        /// Diff selected file versus one of several options, specified in tag field
        /// </summary>
        private void menuViewDiff_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)(sender as ToolStripDropDownItem).Tag;
            App.Repos.current.Run("difftool " + sel.tag + " -- " + sel.selPathGitFormat());
        }

        /// <summary>
        /// Edit selected file using either the default editor (native OS file association,
        /// if the tag is "", or the editor program specified in the tag field
        /// </summary>
        private void menuViewEdit_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)(sender as ToolStripDropDownItem).Tag;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(sel.selPath));
            if (string.IsNullOrEmpty(sel.tag))
                Process.Start(sel.selPath);
            else
                Process.Start(sel.tag, sel.selPath);
        }

        /// <summary>
        /// Run the Windows Explorer in the directory containing a selected file
        /// </summary>
        private void menuViewExplore_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)((ToolStripDropDownItem) sender).Tag;
            Process.Start("explorer.exe", "/e, /select," + sel.selPath);
        }

        /// <summary>
        /// Open a command prompt in the directory containing a selected file
        /// </summary>
        private void menuViewCommand_Click(object sender, EventArgs e)
        {
            TSelection sel = (TSelection)(sender as ToolStripDropDownItem).Tag;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(sel.selPath));
            Process.Start("cmd.exe");
        }
    }
}
