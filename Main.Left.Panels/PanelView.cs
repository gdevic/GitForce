using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

            // Set the view mode text (picked up from the indexed menu combo box)
            viewLabel.Text = dropViewMode.DropDownItems[mode].Text;

            menuSortFilesByExtension.Enabled = btListView.Enabled = App.Repos.Current != null;

            treeView.BeginUpdate();
            treeView.NodesClear();

            if (App.Repos.Current != null)
            {
                Status = App.Repos.Current.Status;

                btListView.Checked = !Status.Repo.IsTreeView;
                menuSortFilesByExtension.Checked = Status.Repo.SortBy == GitDirectoryInfo.SortBy.Extension;

                List<string> files = new List<string>();

                switch (mode)
                {
                    case 0:     // Git status of all files: status + untracked
                        files = Status.GetFiles();
                        break;
                    case 1:     // Git status of files: status - untracked
                        files = Status.GetFiles();
                        // Remove all untracked files
                        files = files.Where(s => Status.Xcode(s) != '?').ToList();
                        break;
                    case 2:     // Git view of repo: ls-tree
                        ExecResult result = App.Repos.Current.Run("ls-tree --abbrev -r -z HEAD");
                        if(result.Success())
                        {
                            string[] response = result.stdout
                                .Replace('/', Path.DirectorySeparatorChar)  // Correct the path slash on Windows
                                .Split(("\0")
                                .ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            files.AddRange(response.Select(s => s.Split('\t').Last()));
                        }
                        break;
                    case 3:     // Local file view: use local directory list
                        files = GitDirectoryInfo.GetFilesRecursive(App.Repos.Current.Root);
                        // Remove the repo root from the file paths
                        int rootlen = App.Repos.Current.Root.Length;
                        files = files.Select(file => file.Substring(rootlen + 1)).ToList();
                        break;
                    case 4:     // Local files not in repo: untracked only
                        files = Status.GetFiles();
                        // Leave only untracked files
                        files = files.Where(s => Status.Xcode(s) == '?').ToList();
                        break;
                }

                // Build the tree view (or a list view)
                TreeNode node = new TreeNode(App.Repos.Current.Root) { Tag = String.Empty };

                if (Status.Repo.IsTreeView)
                    ClassView.BuildTree(node, files, Status.Repo.SortBy);
                else
                    ClassView.BuildFileList(node, files);

                // Add the resulting tree to the tree view control
                treeView.Nodes.Add(node);

                // Assign the icons to the nodes of tree view
                ClassView.ViewAssignIcon(Status, node, false);

                // Set the first node (root) image according to the view mode
                node.ImageIndex = mode == 2
                                      ? (int) ClassView.Img.DatabaseOpened
                                      : (int) ClassView.Img.FolderOpened;

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
            string sel = Path.Combine(App.Repos.Current.Root, selNode.Tag.ToString());
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
                    if (Opclass.ContainsKey(status.Ycode(s)))
                        Opclass[status.Ycode(s)].Add(s);
                    else
                        Opclass[status.Ycode(s)] = new List<string> { s };
                }
            }
        };

        /// <summary>
        /// Handle double-clicking on a tree view
        /// Depending on the saved options, we either do nothing ("0"), open a file
        /// using a default Explorer file association ("1"), or open a file using a
        /// specified application ("2")
        /// </summary>
        private void TreeViewDoubleClick(object sender, EventArgs e)
        {
            string file = GetSelectedFile();
            if (file!=string.Empty)
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
            if (Status == null || treeView.SelectedNodes.Count == 0)
                return;

            // The selection of files contains classes of operations (keys)
            Selection sel = new Selection(treeView, Status);
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
                // TODO: Visually, we add a separator line even if no custom tools are present. This might need fixing.
                contextMenu.Items.Add(new ToolStripSeparator());
                foreach (var tool in App.CustomTools.Tools.Where(tool => tool.IsAddToContextMenu()))
                    contextMenu.Items.Add(new ToolStripMenuItem(tool.Name, null, CustomToolClicked) { Tag = tool });
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

            ToolStripMenuItem mRevHist = new ToolStripMenuItem("Revision History...", null, MenuViewRevHistClick);
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
                mRevHist,
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
            }
            return menu;
        }

        /// <summary>
        /// A specific custom tool is clicked (selected).
        /// Tag contains the tool class.
        /// </summary>
        public void CustomToolClicked(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);

            // Create a list of selected files to send to the Run method
            // Each file needs to have an absolute path, so prepend the repo root
            // These are files and directories, bundled together
            List<string> files = new List<string>();
            foreach(string s in sel.SelFiles)
                files.Add(Path.Combine(App.Repos.Current.Root, s));

            ClassTool tool = (ClassTool)(sender as ToolStripMenuItem).Tag;
            App.PrintStatusMessage(String.Format("{0} {1}", tool.Cmd, tool.Args));
            App.PrintStatusMessage(tool.Run(files));
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
            Selection sel = new Selection(treeView, Status);
            if(sel.Opclass.ContainsKey('?'))
                Status.Repo.GitAdd(sel.Opclass['?']);
            App.DoRefresh();
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
            App.DoRefresh();
        }

        /// <summary>
        /// Update changelist (index) with all files that need updating (disregarding the selection)
        /// TODO: Disregarding the selection?!?
        /// </summary>
        private void MenuViewUpdateAllClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            PanelCommits.DoDropFiles(Status, sel.SelFiles.ToList());
            App.DoRefresh();
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
                App.DoRefresh();
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
                        App.Repos.Current.RunCmd(cmd);
                    App.DoRefresh();
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
            App.DoRefresh();
        }

        /// <summary>
        /// Remove files from the local file system
        /// </summary>
        private void MenuViewRemoveFromFsClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            App.PrintStatusMessage("Removing " + string.Join(" ", sel.SelFiles.ToArray()));

            foreach (string s in sel.SelFiles)
            {
                string fullPath = Path.Combine(App.Repos.Current.Root, s);
                if (ClassUtils.DeleteFile(fullPath) == false)
                    App.PrintStatusMessage("Error removing " + fullPath);
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
                App.PrintStatusMessage("Editing " + file);
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
            if(App.MainForm.InvokeRequired)
                App.MainForm.BeginInvoke((MethodInvoker)(() => OnFileChanged(source, e)));
            else
                App.DoRefresh();
        }

        /// <summary>
        /// Diff selected file versus one of several options, specified in the Tag field
        /// </summary>
        private void MenuViewDiffClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            string opt = (sender as ToolStripMenuItem).Tag.ToString();
            Status.Repo.GitDiff(opt, sel.SelFiles.ToList());
        }

        /// <summary>
        /// Show the revision history dialog for a selected file.
        /// This dialog is _not_ modal, so user can view multiple files.
        /// </summary>
        private void MenuViewRevHistClick(object sender, EventArgs e)
        {
            Selection sel = new Selection(treeView, Status);
            if(sel.SelFiles.Count()==1)
            {
                FormRevisionHistory formRevisionHistory = new FormRevisionHistory(sel.SelFiles[0]);
                formRevisionHistory.Show();
            }
        }
        
        #endregion
    }
}
