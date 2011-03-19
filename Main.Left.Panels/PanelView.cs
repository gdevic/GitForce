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
using GitForce.Main.Right.Panels;

namespace GitForce.Main.Left.Panels
{
    public partial class PanelView : UserControl
    {
        /// <summary>
        /// Status class containing git status of the files in the current repo
        /// </summary>
        private ClassStatus Status;

        /// <summary>
        /// Structure holding the tree selection items information
        /// </summary>
        private class Selection
        {
            public readonly string SelPath;
            public readonly string[] SelFiles;
            public readonly Dictionary<char, List<string>> Opclass = new Dictionary<char, List<string>>();

            public Selection(TreeViewEx treeView, ClassStatus status)
            {
                SelFiles = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();
                SelPath = SelFiles.Count() > 0 ? SelFiles[0] : App.Repos.Current.Root;

                // Move files into different buckets based on what function needs to be done on them
                foreach (string s in SelFiles.Where(status.IsMarked))
                {
                    if (Opclass.ContainsKey(status.GetYcode(s)))
                        Opclass[status.GetYcode(s)].Add("\"" + s + "\"");
                    else
                        Opclass[status.GetYcode(s)] = new List<string> { "\"" + s + "\"" };
                }
            }

            /// <summary>
            /// Returns the list of files (from selFiles) formatted for git file list command --
            /// Absolute paths are shortened into paths relative to the current repo root,
            /// Quotes are added around each file path name,
            /// All files are joined into one resulting string
            /// </summary>
            public string SelFilesGitFormat()
            {
                return String.Join(" ", SelFiles.
                    Select(s => "\"" + s.Substring(App.Repos.Current.Root.Length + 1) + "\"").ToArray());
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
        private void ViewRefresh()
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
            treeView.NodesClear();

            if (App.Repos.Current != null)
            {
                Status = new ClassStatus(App.Repos.Current);

                btListView.Checked = !Status.Repo.IsTreeView;
                menuSortFilesByExtension.Checked = Status.Repo.SortBy == GitDirectoryInfo.SortBy.Extension;

                switch (mode)
                {
                    case 0:     // Git status of all files: status + untracked
                        Status.SetListByStatusCommand("status --porcelain -uall -z");
                        Status.Seal();
                        break;
                    case 1:     // Git status of files: status - untracked
                        Status.SetListByStatusCommand("status --porcelain -uno -z");
                        Status.Filter(s => s.StartsWith("??"));
                        Status.Seal();
                        break;
                    case 2:     // Git view of repo: ls-tree
                        Status.SetListByLsTreeCommand("ls-tree --abbrev -r -z HEAD");
                        Status.ConvertSeal();
                        break;
                    case 3:     // Local file view: use local directory list
                        Status.SetListByStatusCommand("status --porcelain -uall -z");
                        Status.Seal();
                        Status.SetListByList(GitDirectoryInfo.GetFilesRecursive(App.Repos.Current.Root));
                        Status.Seal();
                        break;
                    case 4:     // Local files not in repo: untracked only
                        Status.SetListByStatusCommand("status --porcelain -uall -z");
                        Status.Filter(s => !s.StartsWith("??"));
                        Status.Seal();
                        break;
                }

                // Sort the files in the Status list by the repo's sorting rule
                Status.Sort();

                // Build the tree view (or a list view)
                TreeNode node = new TreeNode(App.Repos.Current.Root);
                node.Tag = App.Repos.Current.Root + Path.DirectorySeparatorChar;

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
        private void TreeViewAfterExpand(object sender, TreeViewEventArgs e)
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
            if (sel != null && !sel.Tag.ToString().EndsWith(Convert.ToString(Path.DirectorySeparatorChar)))
            {
                // Perform the required action on double-click
                string option = Properties.Settings.Default.DoubleClick;
                string program = Properties.Settings.Default.DoubleClickProgram;

                try
                {
                    if (option == "1")
                        Process.Start(sel.Tag.ToString());
                    if (option == "2")
                        Process.Start(program, sel.Tag.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void TreeViewDragEnter(object sender, DragEventArgs e)
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
        /// Operations on a file with regards to the Git system
        /// </summary>
        public enum FileOps { Add, Update, UpdateAll, Revert, Rename, Delete, DeleteFs, Edit, Diff }

        /// <summary>
        /// Contains funcitonal translations of the main menu tool strip file buttons to their handle.
        /// Main form sets this variable with the proper dictionary.
        /// </summary>
        private Dictionary<FileOps, ToolStripButton> StatusButtons;

        public void RegisterToolstripFileButtons(Dictionary<FileOps, ToolStripButton> buttons)
        {
            StatusButtons = buttons;

            // Add handlers to the buttons, these are default handlers for various operations
            Dictionary<FileOps, EventHandler> events = new Dictionary<FileOps, EventHandler>()
            {
                { FileOps.Add,       MenuViewAddFilesClick },
                { FileOps.Update,    MenuViewUpdateChangelistClick },
                { FileOps.UpdateAll, MenuViewUpdateAllClick },
                { FileOps.Revert,    MenuViewRevertClick },
                { FileOps.Rename,    MenuViewRenameClick },
                { FileOps.Delete,    MenuViewOpenForDeleteClick },
                { FileOps.DeleteFs,  MenuViewRemoveFromFsClick },
                { FileOps.Edit,      MenuViewEditClick },
                { FileOps.Diff,      MenuViewDiffClick }
            };
            foreach (var toolStripButton in buttons)
                toolStripButton.Value.Click += events[toolStripButton.Key];
        }

        /// <summary>
        /// Set of operations allowed on a file based on its Git status code
        /// </summary>
        private readonly Dictionary<char, FileOps[]> ops = new Dictionary<char, FileOps[]> {
            {'?', new[]{ FileOps.Add, FileOps.DeleteFs, FileOps.Edit }},
            {' ', new[]{ FileOps.Rename, FileOps.Delete, FileOps.DeleteFs, FileOps.Edit, FileOps.Diff}},
            {'M', new[]{ FileOps.Update, FileOps.UpdateAll, FileOps.Revert, FileOps.Delete, FileOps.DeleteFs, FileOps.Edit, FileOps.Diff}},
            {'D', new[]{ FileOps.Update, FileOps.UpdateAll, FileOps.Revert}},
            {'R', new[]{ FileOps.Update, FileOps.UpdateAll, FileOps.Revert, FileOps.Delete, FileOps.DeleteFs, FileOps.Edit, FileOps.Diff}},
            {'U', new[]{ FileOps.Delete, FileOps.DeleteFs, FileOps.Edit, FileOps.Diff}},
            {'A', new[]{ FileOps.Delete, FileOps.DeleteFs, FileOps.Edit, FileOps.Diff}}
        };

        /// <summary>
        /// Current set of allowed operations on a selected file (or files)
        /// </summary>
        private List<FileOps> allowedOps = new List<FileOps>();

        /// <summary>
        /// Selection changed. Rebuild the set of allowed operations on the current set of selected files.
        /// </summary>
        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Status == null || treeView.SelectedNodes.Count == 0)
                return;

            List<TreeNode> selected = treeView.SelectedNodes;

            // Collect all possible operations for the selected group of nodes
            allowedOps.Clear();
            foreach (var treeNode in selected)
            {
                if (Status.IsMarked(treeNode.Tag.ToString()))
                {
                    char Y = Status.GetYcode(treeNode.Tag.ToString());
                    allowedOps.AddRange(ops[Y]);
                }
            }
            // Remove duplicate entries
            allowedOps = allowedOps.Distinct().ToList();

            // If there is more than one file selected, remove Edit and Diff entries
            if (selected.Count > 1)
                allowedOps.RemoveAll(x => x == FileOps.Edit || x == FileOps.Diff);

            // Enable only chosen status buttons
            foreach (var button in StatusButtons)
                button.Value.Enabled = false;
            foreach (var allowedOp in allowedOps.Where(allowedOp => StatusButtons.ContainsKey(allowedOp)))
                StatusButtons[allowedOp].Enabled = true;
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void TreeViewMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && treeView.Nodes.Count > 0)
            {
                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for files
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            // If there is no current repo, nothing to build
            if (App.Repos.Current == null)
                return new ToolStripItemCollection(owner, new ToolStripItem[] {} );

            ToolStripMenuItem mUpdate = new ToolStripMenuItem("Update Changelist", null, MenuViewUpdateChangelistClick);
            ToolStripMenuItem mRevert = new ToolStripMenuItem("Revert", null, MenuViewRevertClick);

            // Build the "Diff vs" submenu
            ToolStripMenuItem mDiffIndex = new ToolStripMenuItem("Index", null, MenuViewDiffClick) { Tag = "" };
            ToolStripMenuItem mDiffHead = new ToolStripMenuItem("Repository HEAD", null, MenuViewDiffClick) { Tag = "HEAD" };
            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs");
            mDiff.DropDownItems.Add(mDiffIndex);
            mDiff.DropDownItems.Add(mDiffHead);

            // Build the "Edit Using" submenus
            // The default option is to open the file using the OS-associated editor,
            // after which all the user-specified programs are listed
            ToolStripMenuItem mEditAssoc = new ToolStripMenuItem("Associated Editor", null, MenuViewEditClick);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Using");
            mEdit.DropDownItems.Add(mEditAssoc);
            string[] progs = Properties.Settings.Default.EditViewPrograms.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                mEdit.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(s), null, MenuViewEditClick) {Tag = s});

            ToolStripMenuItem mRename = new ToolStripMenuItem("Move/Rename...", null, MenuViewRenameClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Open for Delete", null, MenuViewOpenForDeleteClick);
            ToolStripMenuItem mRemove = new ToolStripMenuItem("Remove from File System", null, MenuViewRemoveFromFsClick);

            ToolStripMenuItem mExplore = new ToolStripMenuItem("Explore...", null, MenuViewExploreClick);
            ToolStripMenuItem mCommand = new ToolStripMenuItem("Command Prompt...", null, MenuViewCommandClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mUpdate,
                mRevert,
                mDiff,
                mEdit,
                new ToolStripSeparator(),
                mRename,
                mDelete,
                mRemove,
                new ToolStripSeparator(),
                mExplore, mCommand
            });

            // Enable only menu items which match the selected file conditions
            // This is a translation dictionary for our menu items
            Dictionary<FileOps, ToolStripMenuItem> menus = new Dictionary<FileOps, ToolStripMenuItem> {
                {FileOps.Update, mUpdate},
                {FileOps.Revert, mRevert},
                {FileOps.Diff, mDiff},
                {FileOps.Edit, mEdit},
                {FileOps.Rename, mRename},
                {FileOps.Delete, mDelete},
                {FileOps.DeleteFs, mRemove},
            };

            // First disable all tracked menu items);
            foreach (var toolStripMenuItem in menus)
                toolStripMenuItem.Value.Enabled = false;

            // Then enable all items which are allowed according to our latest selection
            foreach (var allowedOp in allowedOps.Where(menus.ContainsKey))
                menus[allowedOp].Enabled = true;

            return menu;
        }

        /// <summary>
        /// Run the Windows Explorer in the directory containing a selected file
        /// </summary>
        private void MenuViewExploreClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            string dir = Path.GetDirectoryName(sel.SelPath);
            try
            {
                // WAR: Opening an "Explorer" is platform-specific
                if (ClassUtils.IsMono())
                {
                    // TODO: Start a Linux (Ubuntu?) file explorer in a more flexible way
                    Process.Start(@"/usr/bin/nautilus", "--browser " + dir);
                }
                else
                    Process.Start("explorer.exe", "/e, /select," + sel.SelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Open a command prompt in the directory containing a selected file
        /// </summary>
        private void MenuViewCommandClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            string dir = Path.GetDirectoryName(sel.SelPath);
            if (dir != null)
                ClassUtils.CommandPromptHere(dir);
        }

        #region Handlers for file actions related to Git
        /// Add to Git
        /// Update Changelist
        /// Update all Changed files
        /// Revert
        /// Rename/Move
        /// Delete
        /// Remove from FS
        /// Edit
        /// Diff

        /// <summary>
        /// Add untracked files to Git
        /// </summary>
        private void MenuViewAddFilesClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            if(sel.Opclass.ContainsKey('?'))
                Status.Repo.GitAdd(sel.Opclass['?']);
            App.Refresh();
        }

        /// <summary>
        /// Update changelist (index) with files that are modified, deleted or renamed
        /// </summary>
        private void MenuViewUpdateChangelistClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);

            if (sel.Opclass.ContainsKey('M'))
                Status.Repo.GitUpdate(sel.Opclass['M']);
            if (sel.Opclass.ContainsKey('D'))
                Status.Repo.GitDelete(sel.Opclass['D']);
            if (sel.Opclass.ContainsKey('R'))
                Status.Repo.GitRename(sel.Opclass['R']);

            App.Refresh();
        }

        /// <summary>
        /// Update changelist (index) with all files that need updating (disregarding the selection)
        /// </summary>
        private void MenuViewUpdateAllClick(object sender, EventArgs e)
        {
            // The list of files is not taken from Selection, but from all current repo files
            // Call a generic update function in PanelCommit, but it needs absolute file paths
            List<string> files = Status.GetFileList().Select(x => Path.Combine(Status.Repo.Root, x)).ToList();
            PanelCommits.DoDropFiles(Status, files);
            App.Refresh();
        }

        /// <summary>
        /// Discard changes to selected files
        /// </summary>
        private void MenuViewRevertClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            if (MessageBox.Show("This will revert all changes to selected files in your working directory. It will not affect staged files in Changelists.\r\rProceed with Revert?",
                "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (sel.Opclass.ContainsKey('M'))
                    Status.Repo.GitRevert(sel.Opclass['M']);
                if (sel.Opclass.ContainsKey('D'))
                    Status.Repo.GitRevert(sel.Opclass['D']);
                if (sel.Opclass.ContainsKey('R'))
                    Status.Repo.GitRevert(sel.Opclass['R']);

                ViewRefresh();
            }
        }

        /// <summary>
        /// Open a rename file dialog to rename or move one or a set of files
        /// </summary>
        private void MenuViewRenameClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            FormRename formRename = new FormRename();
            if (formRename.LoadFiles(App.Repos.Current, sel.SelFiles))
                if (formRename.ShowDialog() == DialogResult.OK)
                {
                    List<string> cmds = formRename.GetGitCmds();
                    foreach (string cmd in cmds)
                        App.Repos.Current.Run(cmd);
                }
        }

        /// <summary>
        /// Open files for delete
        /// </summary>
        private void MenuViewOpenForDeleteClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);

            if (sel.Opclass.ContainsKey(' '))
                Status.Repo.GitDelete(sel.Opclass[' ']);
            if (sel.Opclass.ContainsKey('M'))
                Status.Repo.GitDelete(sel.Opclass['M']);
            if (sel.Opclass.ContainsKey('R'))
                Status.Repo.GitDelete(sel.Opclass['R']);

            ViewRefresh();
        }

        /// <summary>
        /// Remove files from the local file system
        /// </summary>
        private void MenuViewRemoveFromFsClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            foreach (string s in sel.SelFiles)
            {
                try
                {
                    File.SetAttributes(s, FileAttributes.Normal);
                    File.Delete(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ViewRefresh();
        }

        /// <summary>
        /// Edit selected file using either the default editor (native OS file association,
        /// if the tag is null, or the editor program specified in the tag field.
        /// This is a handler for both the context menu and the edit tool bar button.
        /// </summary>
        private void MenuViewEditClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(sel.SelPath));
            try
            {
                if (sender is ToolStripMenuItem)
                {
                    object opt = (sender as ToolStripMenuItem).Tag;
                    if (opt != null)
                    {
                        Process.Start(opt.ToString(), sel.SelPath);
                        return;
                    }
                }
                Process.Start(sel.SelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Diff selected file versus one of several options, specified in tag field
        /// </summary>
        private void MenuViewDiffClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            string opt = (sender as ToolStripMenuItem).Tag.ToString();

            App.Repos.Current.Run("difftool " + ClassDiff.GetDiffCmd() + opt + " -- " + sel.SelPathGitFormat());
        }

        #endregion
    }
}
