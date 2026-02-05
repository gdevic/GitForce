using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GitForce.Main.Right.Panels;

namespace GitForce.Main.Left.Panels
{
    public partial class PanelView : UserControl
    {
        /// <summary>
        /// Status class containing git status of the files in the current repo
        /// </summary>
        private ClassStatus status;

        /// <summary>
        /// PanelView constructor
        /// </summary>
        public PanelView()
        {
            InitializeComponent();

            treeView.ImageList = ClassView.GetImageList();

            App.Refresh += ViewRefresh;

            // Initialize the current view
            // Current view mode is persistent and stored in in Properties.Settings.Default.viewMode (int)
            SetView(Properties.Settings.Default.viewMode);

            // Since we use the FullPath property for its intended use (to return a path of a file)
            // set the correct path separator character for the platform ('\' or '/' for Windows and Linux)
            treeView.PathSeparator = Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// Set the view mode by sending a menu item whose Tag contains the mode number.
        /// This function is called from a menu handlers that select view mode.
        /// </summary>
        public void ViewSetByMenuItem(object sender, EventArgs e)
        {
            SetView(int.Parse((sender as ToolStripMenuItem).Tag.ToString()));
            ViewRefresh();
        }

        /// <summary>
        /// Sets the current view mode
        /// </summary>
        public void SetView(int mode)
        {
            // Set the menu bullet to the current view
            List<ToolStripMenuItem> viewMenus = new List<ToolStripMenuItem> {
                menuView0, menuView1, menuView2, menuView3, menuView4, menuView5 };
            foreach (var m in viewMenus)
                m.Checked = false;
            viewMenus[mode].Checked = true;
            Properties.Settings.Default.viewMode = mode;
        }

        /// <summary>
        /// Panel view class refresh function
        /// </summary>
        public void ViewRefresh()
        {
            int mode = Properties.Settings.Default.viewMode;

            // Set the view mode text (picked up from the indexed menu combo box)
            viewLabel.Text = dropViewMode.DropDownItems[mode].Text;

            menuSortFilesByExtension.Enabled = btListView.Enabled = App.Repos.Current != null;

            treeView.BeginUpdate();
            treeView.NodesClear();

            if (App.Repos.Current != null)
            {
                status = App.Repos.Current.Status;

                btListView.Checked = !status.Repo.IsTreeView;
                menuSortFilesByExtension.Checked = status.Repo.SortBy == GitDirectoryInfo.SortBy.Extension;

                List<string> files = new List<string>();

                switch (mode)
                {
                    case 0:     // Git status of all files: status + untracked
                        files = status.GetFiles();
                        break;

                    case 1:     // Git status of files: status - untracked
                        files = status.GetFiles();

                        // Remove all untracked files
                        files = files.Where(s => status.Xcode(s) != '?').ToList();
                        break;

                    case 2:     // Git view of repo: ls-tree
                        ExecResult result = App.Repos.Current.Run("ls-tree --abbrev -r -z HEAD");
                        if (result.Success())
                        {
                            string[] response = result.stdout
                                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                                .Split(("\0")
                                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            // Filter out submodule entries (they show as "commit" type in ls-tree)
                            foreach (string entry in response)
                            {
                                string fileName = entry.Split('\t').Last();
                                // Skip if this is a submodule path
                                if (App.Repos.Current.Submodules == null || !App.Repos.Current.Submodules.IsSubmodule(fileName))
                                    files.Add(fileName);
                            }
                        }
                        // Also add submodules (initialized or not, so user can see and initialize them)
                        if (App.Repos.Current.Submodules != null)
                        {
                            foreach (string smPath in App.Repos.Current.Submodules.GetPaths())
                            {
                                var sm = App.Repos.Current.Submodules.Get(smPath);
                                if (sm.IsInitialized)
                                {
                                    // For initialized submodules, add their contents (folder structure created automatically)
                                    ExecResult smResult = App.Repos.Current.Run("ls-tree --abbrev -r -z HEAD", false, sm.Path);
                                    if (smResult.Success())
                                    {
                                        string[] smResponse = smResult.stdout
                                            .Replace('/', Path.DirectorySeparatorChar)
                                            .Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        // Prefix each file with submodule path
                                        foreach (string entry in smResponse)
                                        {
                                            string fileName = entry.Split('\t').Last();
                                            files.Add(smPath + Path.DirectorySeparatorChar + fileName);
                                        }
                                    }
                                }
                                else
                                {
                                    // For uninitialized submodules, add the path itself so user can see and init it
                                    if (!files.Contains(smPath))
                                        files.Add(smPath);
                                }
                            }
                        }
                        break;

                    case 3:     // Local file view: use local directory list
                        files = GitDirectoryInfo.GetFilesRecursive(App.Repos.Current.Path);

                        // Remove the repo root from the file paths
                        int rootlen = App.Repos.Current.Path.Length;
                        files = files.Select(file => file.Substring(rootlen + 1)).ToList();
                        break;

                    case 4:     // Local files not in repo: untracked only
                        files = status.GetFiles();

                        // Leave only untracked files
                        files = files.Where(s => status.Xcode(s) == '?').ToList();
                        break;

                    case 5:     // Git view local files not in .gitignore
                        ExecResult result2 = App.Repos.Current.Run("ls-files -z --abbrev --exclude-from=.gitignore -o -c -d -m");
                        if (result2.Success())
                        {
                            string[] response2 = result2.stdout
                                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                                .Split(("\0")
                                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            // Filter out submodule entries
                            foreach (string entry in response2)
                            {
                                string fileName = entry.Split('\t').Last();
                                if (App.Repos.Current.Submodules == null || !App.Repos.Current.Submodules.IsSubmodule(fileName))
                                    files.Add(fileName);
                            }
                        }
                        // Also add submodules (initialized or not, so user can see and initialize them)
                        if (App.Repos.Current.Submodules != null)
                        {
                            foreach (string smPath in App.Repos.Current.Submodules.GetPaths())
                            {
                                var sm = App.Repos.Current.Submodules.Get(smPath);
                                if (sm.IsInitialized)
                                {
                                    // For initialized submodules, add their contents (folder structure created automatically)
                                    ExecResult smResult = App.Repos.Current.Run("ls-files -z --abbrev --exclude-from=.gitignore -o -c -d -m", false, sm.Path);
                                    if (smResult.Success())
                                    {
                                        string[] smResponse = smResult.stdout
                                            .Replace('/', Path.DirectorySeparatorChar)
                                            .Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        // Prefix each file with submodule path
                                        foreach (string entry in smResponse)
                                        {
                                            string fileName = entry.Split('\t').Last();
                                            files.Add(smPath + Path.DirectorySeparatorChar + fileName);
                                        }
                                    }
                                }
                                else
                                {
                                    // For uninitialized submodules, add the path itself so user can see and init it
                                    if (!files.Contains(smPath))
                                        files.Add(smPath);
                                }
                            }
                        }
                        break;
                }

                // Build the tree view (or a list view)
                TreeNode node = new TreeNode(App.Repos.Current.Path) { Tag = String.Empty };

                if (status.Repo.IsTreeView)
                    ClassView.BuildTree(node, files, status.Repo.SortBy);
                else
                    ClassView.BuildFileList(node, files, status.Repo.SortBy);

                // Add the resulting tree to the tree view control
                treeView.Nodes.Add(node);

                // Assign the icons to the nodes of tree view
                ClassView.ViewAssignIcon(status, node, false);
                ClassView.ViewUpdateToolTips(node);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = mode == 2
                                      ? (int)ClassView.Img.DatabaseOpened
                                      : (int)ClassView.Img.FolderOpened;

                // Always keep the root node expanded by default
                node.Expand();

                // Finally, expand the rest of the tree to its previous expand state
                ViewExpand(node);
            }
            treeView.ShowNodeToolTips = true;
            treeView.EndUpdate();
        }

        /// <summary>
        /// Refresh the tree view pane
        /// </summary>
        private void MenuRefresh(object sender, EventArgs e)
        {
            // Refresh both this view and the commit pane
            App.MainForm.SelectiveRefresh(FormMain.SelectveRefreshFlags.View | FormMain.SelectveRefreshFlags.Commits);
        }

        /// <summary>
        /// Traverse tree and expand those nodes which are marked as expanded
        /// </summary>
        private static void ViewExpand(TreeNode rootNode)
        {
            TreeNodeCollection nodes = rootNode.Nodes;
            foreach (TreeNode tn in nodes)
            {
                if (App.Repos.Current.IsExpanded(tn.Tag.ToString()))
                {
                    // This will call TreeViewAfterExpand()
                    tn.Expand();
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
        /// Returns the directory path of a selected file or directory
        /// </summary>
        private string GetSelectedDir()
        {
            TreeNode selNode = treeView.SelectedNode ?? treeView.Nodes[0];
            string sel = Path.Combine(App.Repos.Current.Path, selNode.Tag.ToString());
            return Directory.Exists(sel) ? sel : Path.GetDirectoryName(sel);
        }

        /// <summary>
        /// Returns the full path of a single selected file, or empty if no file was selected
        /// </summary>
        private string GetSelectedFile()
        {
            foreach (var tn in treeView.SelectedNodes.Where(tn => File.Exists(tn.FullPath)))
                return tn.FullPath;
            return string.Empty;
        }

        /// <summary>
        /// Structure holding a tree selection items information
        /// </summary>
        private class Selection
        {
            public readonly string[] SelFiles;
            public readonly Dictionary<char, List<string>> Opclass = new Dictionary<char, List<string>>();

            public Selection(TreeViewEx treeView, ClassStatus status)
            {
                SelFiles = treeView.SelectedNodes.Select(s => s.Tag.ToString()).ToArray();

                // Move files into different buckets based on what function needs to be done on them
                foreach (var s in SelFiles)
                {
                    char code = status.Ycode(s);

                    // For files inside submodules, treat as tracked unmodified (' ') if not in main status
                    if (code == '\0' && App.Repos.Current != null && App.Repos.Current.Submodules != null)
                    {
                        ClassSubmodules.Submodule sm;
                        string relativePath;
                        if (App.Repos.Current.Submodules.GetContainingSubmodule(s, out sm, out relativePath))
                            code = ' '; // Treat submodule files as tracked
                    }

                    if (Opclass.ContainsKey(code))
                        Opclass[code].Add(s);
                    else
                        Opclass[code] = new List<string> { s };
                }
            }
        }

        /// <summary>
        /// Handle double-clicking on a tree view
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        private void TreeViewDoubleClick(object sender, EventArgs e)
        {
            string file = GetSelectedFile();
            if (file != string.Empty)
            {
                ClassUtils.FileDoubleClick(file);
                WatchAndRefresh(file);
            }
        }

        /// <summary>
        /// Drag and drop handler. User selected one or more files and started to drag them (away).
        /// Send a set of files through their full path name.
        /// </summary>
        private void TreeViewItemDrag(object sender, ItemDragEventArgs e)
        {
            string[] files = treeView.SelectedNodes.Select(s => s.FullPath.ToString()).ToArray();
            DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Copy);
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
            if (status != null)
                status.ShowTreeInfo(treeView.GetNodeAt(e.X, e.Y));
        }

        /// <summary>
        /// Toggle between list view and tree view.
        /// This button is disabled if there is no repo to view (Status is null)
        /// </summary>
        private void BtListViewClick(object sender, EventArgs e)
        {
            btListView.Checked = status.Repo.IsTreeView;
            status.Repo.IsTreeView = !status.Repo.IsTreeView;
            ViewRefresh();
        }

        /// <summary>
        /// Track the checked setting and refresh the local view when changed
        /// This button is disabled if there is no repo to view (Status is null)
        /// </summary>
        private void MenuSortFilesByExtensionClick(object sender, EventArgs e)
        {
            status.Repo.SortBy = menuSortFilesByExtension.Checked
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
            Dictionary<FileOps, EventHandler> events = new Dictionary<FileOps, EventHandler>
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
            if (status == null || treeView.SelectedNodes.Count == 0)
                return;

            // The selection of files contains classes of operations (keys)
            Selection sel = new Selection(treeView, status);
            List<char> keys = sel.Opclass.Keys.ToList();
            foreach (var key in keys)
                allowedOps.AddRange(ops[key]);

            // Remove duplicate entries
            allowedOps = allowedOps.Distinct().ToList();

            // If there is more than one file selected, remove Edit entries
            if (sel.SelFiles.Length > 1)
                allowedOps.RemoveAll(x => x == FileOps.Edit);

            // Enable only chosen status buttons
            foreach (var button in StatusButtons)
                button.Value.Enabled = false;
            foreach (var allowedOp in allowedOps.Where(allowedOp => StatusButtons.ContainsKey(allowedOp)))
                StatusButtons[allowedOp].Enabled = true;

            App.MainForm.BuildFileMenu();
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

                // Add custom tools
                if (App.CustomTools.Tools.Count > 0)
                {
                    contextMenu.Items.Add(new ToolStripSeparator());
                    foreach (var tool in App.CustomTools.Tools.Where(tool => tool.IsAddToContextMenu))
                        contextMenu.Items.Add(new ToolStripMenuItem(tool.Name, null, CustomToolClicked) { Tag = tool });
                }
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for files
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            ToolStripMenuItem mUpdate = new ToolStripMenuItem("Update Changelist", null, MenuViewUpdateChangelistClick);
            ToolStripMenuItem mRevert = new ToolStripMenuItem("Revert", null, MenuViewRevertClick);

            // Build the "Diff vs" submenu
            ToolStripMenuItem mDiffIndex = new ToolStripMenuItem("Index", null, MenuViewDiffClick) { ShortcutKeys = Keys.Control | Keys.D, Tag = "" };
            ToolStripMenuItem mDiffHead = new ToolStripMenuItem("Repository HEAD", null, MenuViewDiffClick) { ShortcutKeys = Keys.Control | Keys.Shift | Keys.D, Tag = "HEAD" };
            ToolStripMenuItem mDiff = new ToolStripMenuItem("Diff vs");
            mDiff.DropDownItems.Add(mDiffIndex);
            mDiff.DropDownItems.Add(mDiffHead);

            // Build the "Edit Using" submenus
            // The default option is to open the file using the OS-associated editor,
            // after which all the user-specified programs are listed
            ToolStripMenuItem mEditAssoc = new ToolStripMenuItem("Associated Editor", null, MenuViewEditClick) { ShortcutKeys = Keys.Control | Keys.Enter }; // Enter on it's own is better, but is not supported
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit Using");
            mEdit.DropDownItems.Add(mEditAssoc);
            string values = Properties.Settings.Default.EditViewPrograms;
            string[] progs = values.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (progs.Any())
            {
                mEdit.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(progs[0]), null, MenuViewEditClick) { Tag = progs[0], ShortcutKeys = Keys.Control | Keys.Shift | Keys.Enter });
                foreach (string s in progs.Skip(1))
                    mEdit.DropDownItems.Add(new ToolStripMenuItem(Path.GetFileName(s), null, MenuViewEditClick) { Tag = s });
            }

            ToolStripMenuItem mRevHist = new ToolStripMenuItem("Revision History...", null, MenuViewRevHistClick) { ShortcutKeys = Keys.Control | Keys.H };
            ToolStripMenuItem mRename = new ToolStripMenuItem("Move/Rename...", null, MenuViewRenameClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Open for Delete", null, MenuViewOpenForDeleteClick);
            ToolStripMenuItem mRemove = new ToolStripMenuItem("Remove from File System", null, MenuViewRemoveFromFsClick);

            ToolStripMenuItem mExplore = new ToolStripMenuItem("Explore...", null, MenuViewExploreClick);
            ToolStripMenuItem mCommand = new ToolStripMenuItem("Command Prompt...", null, MenuViewCommandClick);

            // Build the "Submodule" submenu (only populated if selection is a submodule)
            ToolStripMenuItem mSubmodule = null;
            if (treeView.SelectedNodes.Count == 1 && App.Repos.Current != null && App.Repos.Current.Submodules != null)
            {
                string selectedPath = treeView.SelectedNodes[0].Tag.ToString().TrimEnd(Path.DirectorySeparatorChar);
                if (App.Repos.Current.Submodules.IsSubmodule(selectedPath))
                {
                    var sm = App.Repos.Current.Submodules.Get(selectedPath);

                    mSubmodule = new ToolStripMenuItem("Submodule");

                    ToolStripMenuItem mSmInit = new ToolStripMenuItem("Initialize && Clone", null, MenuSubmoduleInit)
                        { Tag = selectedPath, Enabled = !sm.IsInitialized };
                    ToolStripMenuItem mSmUpdate = new ToolStripMenuItem("Update", null, MenuSubmoduleUpdate)
                        { Tag = selectedPath, Enabled = sm.IsInitialized };
                    ToolStripMenuItem mSmInfo = new ToolStripMenuItem("Show Info...", null, MenuSubmoduleInfo)
                        { Tag = selectedPath };

                    mSubmodule.DropDownItems.AddRange(new ToolStripItem[] {
                        mSmInit, mSmUpdate,
                        new ToolStripSeparator(),
                        mSmInfo
                    });
                }
            }

            // Build the base menu items list
            List<ToolStripItem> menuItems = new List<ToolStripItem> {
                mUpdate,
                mRevert,
                mDiff,
                mEdit,
                mRevHist,
                new ToolStripSeparator(),
                mRename,
                mDelete,
                mRemove
            };

            // Insert submodule menu if applicable
            if (mSubmodule != null)
            {
                menuItems.Add(new ToolStripSeparator());
                menuItems.Add(mSubmodule);
            }

            menuItems.Add(new ToolStripSeparator());
            menuItems.Add(mExplore);
            menuItems.Add(mCommand);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, menuItems.ToArray());

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

            // If there is no current repo, disable every remaining menu in this collection
            if (App.Repos.Current == null)
            {
                mRevHist.Enabled = mExplore.Enabled = mCommand.Enabled = false;
            }
            else
            {
                // Enable all items which are allowed according to our latest selection
                foreach (var allowedOp in allowedOps.Where(menus.ContainsKey))
                    menus[allowedOp].Enabled = true;

                // A bit of a hack, but revision history on a file is enabled when the edit on a file is enabled
                mRevHist.Enabled = mEdit.Enabled;
            }
            return menu;
        }

        /// <summary>
        /// A specific custom tool is clicked (selected).
        /// Tag contains the tool class.
        /// </summary>
        public void CustomToolClicked(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);

            // Create a list of selected files to send to the Run method
            // Each file needs to have an absolute path, so prepend the repo root
            // These are files and directories, bundled together
            List<string> files = new List<string>();
            foreach (string s in sel.SelFiles)
                files.Add(Path.Combine(App.Repos.Current.Path, s));

            ClassTool tool = (ClassTool)(sender as ToolStripMenuItem).Tag;
            App.PrintStatusMessage(String.Format("{0} {1}", tool.Cmd, tool.Args), MessageType.Command);
            App.PrintStatusMessage(tool.Run(files), MessageType.Output);
        }

        /// <summary>
        /// Run the Windows Explorer in the directory containing a selected file
        /// </summary>
        private void MenuViewExploreClick(object sender, EventArgs e)
        {
            string dir = GetSelectedDir();
            ClassUtils.ExplorerHere(dir, GetSelectedFile());
        }

        /// <summary>
        /// Open command prompt in the directory containing a selected file
        /// </summary>
        private void MenuViewCommandClick(object sender, EventArgs e)
        {
            string dir = GetSelectedDir();
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
        /// Revision history

        /// <summary>
        /// Add untracked files to Git
        /// </summary>
        private void MenuViewAddFilesClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            if (sel.Opclass.ContainsKey('?'))
                status.Repo.GitAdd(sel.Opclass['?']);
            App.DoRefresh();
        }

        /// <summary>
        /// Update changelist (index) with files that are modified, deleted or renamed
        /// </summary>
        private void MenuViewUpdateChangelistClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            if (sel.Opclass.ContainsKey('M'))
                status.Repo.GitUpdate(sel.Opclass['M']);
            if (sel.Opclass.ContainsKey('D'))
                status.Repo.GitDelete(sel.Opclass['D']);
            if (sel.Opclass.ContainsKey('R'))
                status.Repo.GitRename(sel.Opclass['R']);
            App.DoRefresh();
        }

        /// <summary>
        /// Update changelist (index) with all files that need updating (disregarding the selection)
        /// TODO: Disregarding the selection?!?
        /// </summary>
        private void MenuViewUpdateAllClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            PanelCommits.DoDropFiles(status, sel.SelFiles.ToList());
            App.DoRefresh();
        }

        /// <summary>
        /// Discard changes to selected files
        /// </summary>
        private void MenuViewRevertClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            if (MessageBox.Show("This will revert all changes to selected files in your working directory. It will not affect staged files in Changelists.\r\rProceed with Revert?",
                "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (sel.Opclass.ContainsKey('M'))
                    status.Repo.GitRevert(sel.Opclass['M']);
                if (sel.Opclass.ContainsKey('D'))
                    status.Repo.GitRevert(sel.Opclass['D']);
                if (sel.Opclass.ContainsKey('R'))
                    status.Repo.GitRevert(sel.Opclass['R']);
                App.DoRefresh();
            }
        }

        /// <summary>
        /// Open a rename file dialog to rename or move one or a set of files
        /// </summary>
        private void MenuViewRenameClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            FormRename formRename = new FormRename();
            if (formRename.LoadFiles(App.Repos.Current, sel.SelFiles))
                if (formRename.ShowDialog() == DialogResult.OK)
                {
                    List<string> cmds = formRename.GetGitCmds();
                    foreach (string cmd in cmds)
                        App.Repos.Current.RunCmd(cmd);
                    App.DoRefresh();
                }
        }

        /// <summary>
        /// Open files for delete
        /// </summary>
        private void MenuViewOpenForDeleteClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            if (sel.Opclass.ContainsKey(' '))
                status.Repo.GitDelete(sel.Opclass[' ']);
            if (sel.Opclass.ContainsKey('M'))
                status.Repo.GitDelete(sel.Opclass['M']);
            if (sel.Opclass.ContainsKey('R'))
                status.Repo.GitDelete(sel.Opclass['R']);
            App.DoRefresh();
        }

        /// <summary>
        /// Remove files from the local file system
        /// </summary>
        private void MenuViewRemoveFromFsClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            App.PrintStatusMessage("Removing " + string.Join(" ", sel.SelFiles.ToArray()), MessageType.General);

            foreach (string s in sel.SelFiles)
            {
                string fullPath = Path.Combine(App.Repos.Current.Path, s);
                if (ClassUtils.DeleteFile(fullPath) == false)
                    App.PrintStatusMessage("Error removing " + fullPath, MessageType.Error);
            }
            App.DoRefresh();
        }

        /// <summary>
        /// Edit selected file using either the default editor (native OS file association,
        /// if the tag is null, or the editor program specified in the tag field.
        /// This is a handler for both the context menu and the edit tool bar button.
        /// </summary>
        private void MenuViewEditClick(object sender, EventArgs e)
        {
            string file = GetSelectedFile();
            if (file != string.Empty)
            {
                App.PrintStatusMessage("Editing " + file, MessageType.General);
                ClassUtils.FileOpenFromMenu(sender, file);
                WatchAndRefresh(file);
            }
        }

        /// <summary>
        /// Sets up a file watcher in order to refresh the view when a file is
        /// being modified by an external editor.
        /// </summary>
        private void WatchAndRefresh(string file)
        {
            // Watch the changes to that file, so we can refresh the view
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(file);
            watcher.Filter = Path.GetFileName(file);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Called by the callback thread when a file that has been watched was changed.
        /// We are watching files that user edits in an external editor.
        /// </summary>
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            if (App.MainForm.InvokeRequired)
                App.MainForm.BeginInvoke((MethodInvoker)(() => OnFileChanged(source, e)));
            else
                App.DoRefresh();
        }

        /// <summary>
        /// Diff selected file versus one of several options, specified in the Tag field
        /// </summary>
        private void MenuViewDiffClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            string opt = (sender as ToolStripMenuItem).Tag.ToString();
            status.Repo.GitDiff(opt, sel.SelFiles.ToList());
        }

        /// <summary>
        /// Show the revision history dialog for a selected file.
        /// This dialog is _not_ modal, so user can view multiple files.
        /// </summary>
        private void MenuViewRevHistClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, status);
            if (sel.SelFiles.Count() == 1)
            {
                FormRevisionHistory formRevisionHistory = new FormRevisionHistory(sel.SelFiles[0]);
                formRevisionHistory.Show();
            }
        }

        #endregion Handlers for file actions related to Git

        #region Submodule handlers

        /// <summary>
        /// Initialize and clone a submodule
        /// </summary>
        private void MenuSubmoduleInit(object sender, EventArgs e)
        {
            string path = (sender as ToolStripMenuItem).Tag.ToString();
            App.PrintStatusMessage("Initializing and cloning submodule: " + path, MessageType.General);
            status.Repo.RunCmd("submodule update --init --force \"" + path + "\"");
            App.DoRefresh();
        }

        /// <summary>
        /// Update a submodule to the recorded commit
        /// </summary>
        private void MenuSubmoduleUpdate(object sender, EventArgs e)
        {
            string path = (sender as ToolStripMenuItem).Tag.ToString();
            App.PrintStatusMessage("Updating submodule: " + path, MessageType.General);
            status.Repo.RunCmd("submodule update \"" + path + "\"");
            App.DoRefresh();
        }

        /// <summary>
        /// Show submodule information dialog
        /// </summary>
        private void MenuSubmoduleInfo(object sender, EventArgs e)
        {
            string path = (sender as ToolStripMenuItem).Tag.ToString();
            var sm = App.Repos.Current.Submodules.Get(path);

            FormSubmoduleInfo formSubmoduleInfo = new FormSubmoduleInfo();
            formSubmoduleInfo.LoadSubmodule(sm);
            formSubmoduleInfo.ShowDialog();
        }

        #endregion Submodule handlers
    }
}
