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
        /// Status class containing the git status of current repo files
        /// </summary>
        private ClassStatus Status;

        /// <summary>
        /// Structure holding the tree selection items information
        /// </summary>
        private struct Selection
        {
            public string SelPath;
            public string[] SelFiles;
            public string tag;

            /// <summary>
            /// Returns the list of files (from selFiles) formatted for git file list command --
            /// Absolute paths are shortened into paths relative to the current repo root,
            /// Quotes are added around each file path name,
            /// All files are joined into one resulting string
            /// </summary>
            public string SelFilesGitFormat()
            {
                return String.Join(" ", SelFiles.
                    Select(s => "\"" + s.Substring(App.Repos.Current.Root.Length + 1) + "\"").ToList());
            }

            /// <summary>
            /// Returns the selPath file formatted for git file list command --
            /// Absolute path is shortened into path relative to the current repo root,
            /// Quotes are added around the file path name
            /// </summary>
            /// <returns></returns>
            public string SelPathGitFormat()
            {
                return "\"" + SelPath.Substring(App.Repos.Current.Root.Length + 1) + "\"";
            }
        };

        public PanelView()
        {
            InitializeComponent();

            treeView.ImageList = ClassView.GetImageList();

            App.Refresh += ViewRefresh;

            // Initialize the current view
            // Current view mode is persistent and stored in in Properties.Settings.Default.viewMode (int)
            SetView(Properties.Settings.Default.viewMode);
        }

        /// <summary>
        /// Set the view mode by sending a menu item whose Tag contains the mode number.
        /// This function is called from a menu handlers that select view mode.
        /// </summary>
        public void ViewSetByMenuItem(object sender, EventArgs e)
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
            ViewRefresh();
        }

        /// <summary>
        /// Panel view class refresh function
        /// </summary>
        public void ViewRefresh()
        {
            int mode = Properties.Settings.Default.viewMode;

            // Define root initial icons for 5 viewing modes
            int[] icons = { (int)ClassView.Img.FolderOpened, 
                            (int)ClassView.Img.FolderOpened, 
                            (int)ClassView.Img.DatabaseOpened,
                            (int)ClassView.Img.FolderOpened,
                            (int)ClassView.Img.FolderOpened };

            // Set the view mode text (picked up from the indexed menu combo box)
            viewLabel.Text = dropViewMode.DropDownItems[mode].Text;

            menuSortFilesByExtension.Enabled = btListView.Enabled = App.Repos.Current != null;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            if (App.Repos.Current != null)
            {
                Status = new ClassStatus(App.Repos.Current);

                btListView.Checked = !Status.Repo.IsTreeView;
                menuSortFilesByExtension.Checked = Status.Repo.SortBy == GitDirectoryInfo.SortBy.Extension;

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
                        Status.SetListByList(GitDirectoryInfo.GetFilesRecursive(App.Repos.Current.Root));
                        Status.Seal();
                        break;
                    case 4:     // Local files not in repo: untracked only
                        Status.SetListByCommand("status --porcelain -uall -z *");
                        Status.Filter(s => !s.StartsWith("??"));
                        Status.Seal();
                        break;
                }

                // Sort the files in the Status list by the repo's sorting rule
                Status.Sort();

                // Build the tree view (or a list view)
                TreeNode node = new TreeNode(App.Repos.Current.Root);
                node.Tag = App.Repos.Current.Root + @"\";

                if (Status.Repo.IsTreeView)
                    ClassView.BuildTreeRecurse(node, Status.GetFileList(), Status.Repo.SortBy);
                else
                    ClassView.BuildFileList(node, App.Repos.Current.Root, Status.GetFileList());

                // Add the resulting tree to the tree view control
                treeView.Nodes.Add(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = icons[mode];

                // Assign the icons to the nodes of tree view
                ClassView.ViewAssignIcon(Status, node, false);

                // Always keep the root node expanded by default
                node.Expand();

                // Finally, expand the rest of the tree to its previous expand state
                ViewExpand(node);

            }
            treeView.EndUpdate();
        }

        /// <summary>
        /// Refresh the tree view pane
        /// </summary>
        private void MenuRefresh(object sender, EventArgs e)
        {
            ViewRefresh();
        }

        /// <summary>
        /// Traverse tree and expand those nodes which are marked as expanded
        /// </summary>
        private static void ViewExpand(TreeNode rootNode)
        {
            TreeNodeCollection nodes = rootNode.Nodes;
            foreach (TreeNode tn in nodes)
            {
                if( App.Repos.Current.IsExpanded(tn.Tag.ToString()))
                {
                    tn.Expand();
                    tn.ImageIndex |= 1;
                }
                ViewExpand(tn);
            }
        }

        /// <summary>
        /// Callback called when user clicks on a node to expand it
        /// </summary>
        private static void TreeViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            App.Repos.Current.ExpansionSet(tn.Tag.ToString());
            tn.ImageIndex |= 1;
        }

        /// <summary>
        /// Callback called when user clicks on a node to collapse it
        /// </summary>
        private void TreeViewAfterCollapse(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            // If the user closed the root node, collapse everything
            if (tn == treeView.Nodes[0])
            {
                App.Repos.Current.ExpansionReset(null);
                treeView.CollapseAll();
            }
            else
                App.Repos.Current.ExpansionReset(tn.Tag.ToString());
            tn.ImageIndex &= ~1;
        }

        /// <summary>
        /// Handle double-clicking on a tree view
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        private void TreeViewDoubleClick(object sender, EventArgs e)
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
                ViewRefresh();
            }
        }

        /// <summary>
        /// Drag and drop handler. User selected one or more files and started to drag them (away).
        /// Send a set of files through their full path name.
        /// </summary>
        private void TreeViewItemDrag(object sender, ItemDragEventArgs e)
        {
            string[] files = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();
            DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Move);
        }

        private static void TreeViewDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// As the mouse moves over nodes, show the human readable description of
        /// files that the mouse points to
        /// </summary>
        private void TreeViewMouseMove(object sender, MouseEventArgs e)
        {
            if (Status != null)
                Status.ShowTreeInfo(treeView.GetNodeAt(e.X, e.Y));
        }

        /// <summary>
        /// Toggle between list view and tree view.
        /// This button is disabled if there is no repo to view (Status is null)
        /// </summary>
        private void BtListViewClick(object sender, EventArgs e)
        {
            btListView.Checked = Status.Repo.IsTreeView;
            Status.Repo.IsTreeView = !Status.Repo.IsTreeView;
            ViewRefresh();
        }

        /// <summary>
        /// Track the checked setting and refresh the local view when changed
        /// This button is disabled if there is no repo to view (Status is null)
        /// </summary>
        private void MenuSortFilesByExtensionClick(object sender, EventArgs e)
        {
            Status.Repo.SortBy = menuSortFilesByExtension.Checked
                                     ? GitDirectoryInfo.SortBy.Extension
                                     : GitDirectoryInfo.SortBy.Name;
            ViewRefresh();
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
        private void TreeViewMouseUp(object sender, MouseEventArgs e)
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
        private static ToolStripMenuItem CreateMenu(string name, EventHandler onClick, Selection sel, string tag="")
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
            if (App.Repos.Current == null)
                return new ToolStripItemCollection(owner, new ToolStripItem[] {} );

            Selection sel = new Selection();
            sel.SelFiles = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();

            // Empty path is only sent from the main menu (not the context right-click handler)
            // in which case consider selected file first file in the selected list
            if (string.IsNullOrEmpty(path))
                sel.SelPath = sel.SelFiles.Count() > 0? 
                    sel.SelPath = sel.SelFiles[0] :
                    App.Repos.Current.Root;
            else
                sel.SelPath = path;

            App.Execute.Add(sel.SelPath + Environment.NewLine);

            ToolStripMenuItem mUpdate = CreateMenu("Update Changelist", MenuViewUpdateChangelistClick, sel);
            ToolStripMenuItem mRevert = CreateMenu("Revert", MenuViewRevertClick, sel);


            // Build the "Diff vs" submenu
            ToolStripMenuItem mDiffIndex = CreateMenu("Index", MenuViewDiffClick, sel);
            ToolStripMenuItem mDiffHead = CreateMenu("Repository HEAD", MenuViewDiffClick, sel, "HEAD");
            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs");
            mDiff.DropDownItems.Add(mDiffIndex);
            mDiff.DropDownItems.Add(mDiffHead);

            // Build the "Edit Using" submenus
            // The default option is to open the file using the OS-associated editor,
            // after which all the user-specified programs are listed
            ToolStripMenuItem mEditAssoc = CreateMenu("Associated Editor", MenuViewEditClick, sel);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Using");
            mEdit.DropDownItems.Add(mEditAssoc);
            string[] progs = Properties.Settings.Default.EditViewPrograms.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                mEdit.DropDownItems.Add(CreateMenu(Path.GetFileName(s), MenuViewEditClick, sel, s));

            ToolStripMenuItem mExplore = CreateMenu("Explore...", MenuViewExploreClick, sel);
            ToolStripMenuItem mCommand = CreateMenu("Command Prompt...", MenuViewCommandClick, sel);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mUpdate,
                mRevert,
                mDiff,
                mEdit,
                new ToolStripSeparator(),
                mExplore, mCommand
            });

            if (sel.SelPath.EndsWith(@"\"))
                mRevert.Enabled = 
                mDiff.Enabled =
                mEdit.Enabled = false;

            return menu;
        }

        /// <summary>
        /// Update changelist (index) with selected files
        /// </summary>
        private void MenuViewUpdateChangelistClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)(sender as ToolStripDropDownItem).Tag;
            Status.Repo.Run("add -- " + sel.SelFilesGitFormat());
            App.Refresh();      // App-wide refresh since 'add' modifies other panes
        }

        /// <summary>
        /// Discard changes to selected files in the working set
        /// </summary>
        private void MenuViewRevertClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)(sender as ToolStripDropDownItem).Tag;
            if (MessageBox.Show("This will revert all changes to selected files in your working directory. It will not affect staged files in Changelists.\r\rProceed with Revert?",
                "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Status.Repo.Run("checkout -- " + sel.SelFilesGitFormat());
                ViewRefresh();
            }
        }

        /// <summary>
        /// Diff selected file versus one of several options, specified in tag field
        /// </summary>
        private static void MenuViewDiffClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)(sender as ToolStripDropDownItem).Tag;
            App.Repos.Current.Run("difftool " + sel.tag + " -- " + sel.SelPathGitFormat());
        }

        /// <summary>
        /// Edit selected file using either the default editor (native OS file association,
        /// if the tag is "", or the editor program specified in the tag field
        /// </summary>
        private static void MenuViewEditClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)(sender as ToolStripDropDownItem).Tag;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(sel.SelPath));
            if (string.IsNullOrEmpty(sel.tag))
                Process.Start(sel.SelPath);
            else
                Process.Start(sel.tag, sel.SelPath);
        }

        /// <summary>
        /// Run the Windows Explorer in the directory containing a selected file
        /// </summary>
        private static void MenuViewExploreClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)((ToolStripDropDownItem) sender).Tag;
            Process.Start("explorer.exe", "/e, /select," + sel.SelPath);
        }

        /// <summary>
        /// Open a command prompt in the directory containing a selected file
        /// </summary>
        private static void MenuViewCommandClick(object sender, EventArgs e)
        {
            Selection sel = (Selection)(sender as ToolStripDropDownItem).Tag;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(sel.SelPath));
            Process.Start("cmd.exe");
        }
    }
}
