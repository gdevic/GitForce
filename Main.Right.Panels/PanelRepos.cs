using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GitForce.Main.Right.Panels
{
    public partial class PanelRepos : UserControl
    {
        enum RepoIcons { RepoIdle, RepoDefault, RepoCurrent, RepoBoth, ProjectClosed, ProjectOpen }

        /// <summary>
        /// True when using flat ListView mode, false for tree mode with project grouping
        /// </summary>
        private bool UseFlatMode
        {
            get { return Properties.Settings.Default.UseFlatRepoList; }
        }

        public PanelRepos()
        {
            InitializeComponent();

            // Add folder icons to the image list for project nodes
            imageList.Images.Add("ProjectClosed", Properties.Resources.TreeIconFC);
            imageList.Images.Add("ProjectOpen", Properties.Resources.TreeIconFO);

            // Load the Scroll icon for the flat/tree toggle button
            btFlatList.Image = new Icon(Properties.Resources.Scroll, 16, 16).ToBitmap();

            // Set initial checked state from setting
            btFlatList.Checked = UseFlatMode;

            App.Refresh += ReposRefresh;
        }

        /// <summary>
        /// Toggle between flat list and tree view with projects
        /// </summary>
        private void BtFlatListClick(object sender, EventArgs e)
        {
            // Capture selection from the outgoing view before switching
            List<ClassRepo> selectedRepos = GetSelectedRepos();

            Properties.Settings.Default.UseFlatRepoList = btFlatList.Checked;
            ReposRefresh();

            // Restore selection in the incoming view
            RestoreSelection(selectedRepos);
        }

        #region Refresh

        /// <summary>
        /// Fill in the list of repositories using the active mode.
        /// </summary>
        public void ReposRefresh()
        {
            if (UseFlatMode)
            {
                listRepos.Visible = true;
                treeRepos.Visible = false;
                FlatRefresh();
            }
            else
            {
                listRepos.Visible = false;
                treeRepos.Visible = true;
                TreeRefresh();
            }
        }

        /// <summary>
        /// Flat mode: populate the ListView (original behavior)
        /// </summary>
        private void FlatRefresh()
        {
            listRepos.BeginUpdate();
            listRepos.Items.Clear();
            foreach (ClassRepo r in App.Repos.Repos)
            {
                ListViewItem li = new ListViewItem(r.Path);
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, r.UserName));
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, r.UserEmail));

                // List items' tags are actual pointers to repos' individual objects
                li.Tag = r;
                li.ImageIndex = (int)RepoIcons.RepoIdle;

                // Select the 'current' repository in the list
                if (r == App.Repos.Current)
                {
                    li.Selected = true;
                    li.ImageIndex |= 2; // Bit [1] is current
                }

                if (r == App.Repos.Default)
                    li.ImageIndex |= 1; // Bit [0] is default

                listRepos.Items.Add(li);
            }
            listRepos.EndUpdate();

            AdjustHeaderColumnWidths();
        }

        /// <summary>
        /// Tree mode: populate the TreeView with projects and repos
        /// </summary>
        private void TreeRefresh()
        {
            treeRepos.BeginUpdate();
            treeRepos.NodesClear();

            var layout = App.Repos.ProjectLayout;
            TreeNode nodeToSelect = null;

            foreach (string entry in layout.RootOrder)
            {
                if (entry.StartsWith("P:"))
                {
                    string projectName = entry.Substring(2);
                    ClassProject project = layout.FindProject(projectName);
                    if (project == null) continue;

                    TreeNode projectNode = new TreeNode(projectName);
                    int projIcon = project.IsExpanded ? (int)RepoIcons.ProjectOpen : (int)RepoIcons.ProjectClosed;
                    projectNode.ImageIndex = projectNode.SelectedImageIndex = projIcon;
                    projectNode.Tag = project;

                    foreach (string repoPath in project.RepoPaths)
                    {
                        ClassRepo repo = App.Repos.Find(repoPath);
                        if (repo == null) continue;
                        TreeNode repoNode = CreateRepoNode(repo);
                        projectNode.Nodes.Add(repoNode);
                        if (repo == App.Repos.Current)
                            nodeToSelect = repoNode;
                    }
                    treeRepos.Nodes.Add(projectNode);

                    // Restore expanded/collapsed state
                    if (project.IsExpanded)
                        projectNode.Expand();
                    else
                        projectNode.Collapse();
                }
                else if (entry.StartsWith("R:"))
                {
                    string repoPath = entry.Substring(2);
                    ClassRepo repo = App.Repos.Find(repoPath);
                    if (repo == null) continue;
                    TreeNode repoNode = CreateRepoNode(repo);
                    treeRepos.Nodes.Add(repoNode);
                    if (repo == App.Repos.Current)
                        nodeToSelect = repoNode;
                }
            }

            treeRepos.EndUpdate();

            if (nodeToSelect != null)
                treeRepos.SelectedNodes = new List<TreeNode> { nodeToSelect };
        }

        /// <summary>
        /// Create a tree node for a repo with inline info text
        /// </summary>
        private TreeNode CreateRepoNode(ClassRepo repo)
        {
            string text = repo.Path;

            int icon = (int)RepoIcons.RepoIdle;
            if (repo == App.Repos.Current) icon |= 2;
            if (repo == App.Repos.Default) icon |= 1;

            TreeNode node = new TreeNode(text);
            node.ImageIndex = node.SelectedImageIndex = icon;
            node.Tag = repo;

            return node;
        }

        #endregion

        #region Flat mode handlers (existing ListView code)

        /// <summary>
        /// Save columns' widths every time they change.
        /// </summary>
        private void ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (listRepos.Items.Count > 0)
            {
                var columns = new int[4];
                foreach (ColumnHeader l in listRepos.Columns)
                {
                    if (l.Width <= 0)
                        return;
                    columns[l.Index] = l.Width;
                }
                string values = String.Join(",", columns.Select(i => i.ToString()).ToArray());
                Properties.Settings.Default.ReposColumnWidths = values;
            }
        }

        /// <summary>
        /// Double-clicking on a repository will switch to it.
        /// </summary>
        private void ListReposDoubleClick(object sender, EventArgs e)
        {
            App.Repos.SetCurrent(GetSelectedRepo());
            App.DoRefresh();
        }

        /// <summary>
        /// Right-mouse button opens a popup with the context menu (flat mode)
        /// </summary>
        private void ListReposMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetFlatContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Builds and returns a context menu for flat mode
        /// </summary>
        private ToolStripItemCollection GetFlatContextMenu(ToolStrip owner)
        {
            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, MenuNewRepoClick);
            ToolStripMenuItem mScan = new ToolStripMenuItem("Scan...", null, MenuScanRepoClick);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit...", null, MenuRepoEditClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, MenuDeleteRepoClick);
            ToolStripMenuItem mClone = new ToolStripMenuItem("Clone...", null, MenuNewRepoClick);
            ToolStripMenuItem mSwitchTo = new ToolStripMenuItem("Switch to...", null, ListReposDoubleClick);
            ToolStripMenuItem mSetDefault = new ToolStripMenuItem("Set Default to...", null, MenuSetDefaultRepoToClick);
            ToolStripMenuItem mCommand = new ToolStripMenuItem("Command Prompt...", null, MenuViewCommandClick);
            ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mScan, mEdit, mDelete, mClone,
                new ToolStripSeparator(),
                mSwitchTo, mSetDefault, mCommand,
                new ToolStripSeparator(),
                mRefresh
            });

            ClassRepo repo = GetSelectedRepo();

            if (repo == null)
                mEdit.Enabled = mDelete.Enabled = mClone.Enabled = mSwitchTo.Enabled = mSetDefault.Enabled = mCommand.Enabled = false;
            else
            {
                mNew.Tag = String.Empty;
                if (repo == App.Repos.Current)
                    mSwitchTo.Enabled = false;

                if (repo == App.Repos.Default)
                    mSetDefault.Enabled = false;
            }
            if (listRepos.SelectedIndices.Count > 1)
            {
                mDelete.Enabled = true;
                mCommand.Enabled = false;
                mClone.Enabled = false;
            }

            if (listRepos.SelectedIndices.Count == 1)
            {
                string repoName = listRepos.SelectedItems[0].Text.Replace('\\', '/').Split('/').Last();
                mSwitchTo.Text = "Switch to " + repoName;
                mSetDefault.Text = "Set Default to " + repoName;
                mClone.Tag = App.Repos.Repos[listRepos.SelectedIndices[0]];
            }

            return menu;
        }

        /// <summary>
        /// This method handles dropping objects into our listview and reordiring the list of repos
        /// We handle 2 cases:
        /// 1. Dropping one or more folders that contain roots of the git repos in order to add repos to the list
        /// 2. Dropping an existing repo name from the listbox itself in order to reorder the list
        /// </summary>
        private void ListReposDragDrop(object sender, DragEventArgs e)
        {
            // User is dropping one or more valid git directory onto the list
            if (e.Effect == DragDropEffects.Copy)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<string> repos = ValidGitRepos(data);
                foreach (string repo in repos)
                {
                    // Add each repo from the list of valid repos. However, open the repo edit
                    // dialog only if the user is adding only a single repo (and not multiple)
                    App.PrintLogMessage("DropRepo: " + repo, MessageType.Debug);
                    AddNewRepo(repo, repos.Count == 1);
                }
            }
            // User is dropping a single, non-git folder; create a new git repo within that folder
            if (e.Effect == DragDropEffects.Link)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (data != null && data.Length == 1)
                {
                    string repo = data[0];
                    App.PrintLogMessage("DropRepo: " + repo, MessageType.Debug);
                    string root = NewRepoWizard(null, null, repo);
                    if (!string.IsNullOrEmpty(root))
                        AddNewRepo(root, true);
                }
            }
            // User is dropping another list view item; he is reordering the list
            if (e.Effect == DragDropEffects.Move)
            {
                // Form a list of names by reading them from the listview
                List<string> names = listRepos.Items.Cast<ListViewItem>()
                    .Select(item => item.Text)
                    .ToList();
                App.Repos.SetOrder(names);
            }
        }

        /// <summary>
        /// This method is called when an object is being dragged onto the control
        /// If the user drags in one or more valid git root directories, we will add corresponding repos
        /// </summary>
        private void ListReposDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (ValidGitRepos(data).Count > 0)
                {
                    // Use Copy effect for outside drop of existing repo(s) (see TreeViewEx limitation)
                    e.Effect = DragDropEffects.Copy;
                }
                if (data != null && data.Length == 1 && ClassUtils.DirStat(data[0]) == ClassUtils.DirStatType.Nongit)
                    e.Effect = DragDropEffects.Link; // Use Link effect for dropping a non-git folder to create a new repo
            }
        }

        /// <summary>
        /// The only purpose of this handler is to fix a Linux listview issue where
        /// the header is sometimes not visible when a tab is switched to
        /// </summary>
        private void ListReposVisibleChanged(object sender, EventArgs e)
        {
            if (!ClassUtils.IsMono()) return; // Linux/Mono fixup only
            if (!Visible) return; // Only on becoming visible
            AdjustHeaderColumnWidths();
        }

        /// <summary>
        /// Adjust header column widths based on the propertiy setting value
        /// </summary>
        private void AdjustHeaderColumnWidths()
        {
            listRepos.BeginUpdate();

            // Adjust the header columns
            string values = Properties.Settings.Default.ReposColumnWidths;
            int[] columns = { -2, -2, -2, 0 }; // Auto-adjust by default
            if (!string.IsNullOrEmpty(values)) // Otherwise, load widths from the settings
                columns = values.Split(',').Select(Int32.Parse).ToArray();
            foreach (ColumnHeader l in listRepos.Columns)
                l.Width = (columns[l.Index] <= 0) ? -1 : columns[l.Index]; // Additional safeguard against storing invalid column widths

            listRepos.EndUpdate();
        }

        #endregion

        #region Tree mode handlers

        /// <summary>
        /// Double-click in tree mode: switch to repo, or toggle project expand
        /// </summary>
        private void TreeReposDoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeRepos.SelectedNode;
            if (node == null) return;

            if (node.Tag is ClassRepo)
            {
                App.Repos.SetCurrent((ClassRepo)node.Tag);
                App.DoRefresh();
            }
        }

        /// <summary>
        /// Right-click in tree mode
        /// </summary>
        private void TreeReposMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = treeRepos.GetNodeAt(e.X, e.Y);

                // If right-clicking a node not already in the selection, select only that node.
                // If right-clicking empty space, clear the selection.
                if (clickedNode != null)
                {
                    if (!treeRepos.SelectedNodes.Contains(clickedNode))
                        treeRepos.SelectedNodes = new List<TreeNode> { clickedNode };
                }
                else
                {
                    treeRepos.SelectedNodes = new List<TreeNode>();
                }

                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetTreeContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Persist expand/collapse state for project nodes
        /// </summary>
        private void TreeReposAfterCollapseExpand(object sender, TreeViewEventArgs e)
        {
            ClassProject project = e.Node.Tag as ClassProject;
            if (project != null)
            {
                project.IsExpanded = e.Node.IsExpanded;
                int icon = e.Node.IsExpanded ? (int)RepoIcons.ProjectOpen : (int)RepoIcons.ProjectClosed;
                e.Node.ImageIndex = e.Node.SelectedImageIndex = icon;
            }
        }

        /// <summary>
        /// Builds context menu for tree mode based on selected node type
        /// </summary>
        private ToolStripItemCollection GetTreeContextMenu(ToolStrip owner)
        {
            TreeNode selectedNode = treeRepos.SelectedNode;
            bool isProjectNode = selectedNode != null && selectedNode.Tag is ClassProject;
            bool isRepoNode = selectedNode != null && selectedNode.Tag is ClassRepo;
            bool isRepoInProject = isRepoNode && selectedNode.Parent != null && selectedNode.Parent.Tag is ClassProject;

            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, MenuNewRepoClick);
            ToolStripMenuItem mScan = new ToolStripMenuItem("Scan...", null, MenuScanRepoClick);
            ToolStripMenuItem mNewProject = new ToolStripMenuItem("New Project...", null, MenuNewProjectClick);
            ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);

            if (isProjectNode)
            {
                ToolStripMenuItem mRenameProject = new ToolStripMenuItem("Rename Project...", null, MenuRenameProjectClick);
                ToolStripMenuItem mDeleteProject = new ToolStripMenuItem("Delete Project", null, MenuDeleteProjectClick);

                return new ToolStripItemCollection(owner, new ToolStripItem[] {
                    mRenameProject, mDeleteProject,
                    new ToolStripSeparator(),
                    mNew, mScan, mNewProject,
                    new ToolStripSeparator(),
                    mRefresh
                });
            }

            if (isRepoNode)
            {
                ClassRepo repo = (ClassRepo)selectedNode.Tag;

                ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit...", null, MenuRepoEditClick);
                ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, MenuDeleteRepoClick);
                ToolStripMenuItem mClone = new ToolStripMenuItem("Clone...", null, MenuNewRepoClick);
                ToolStripMenuItem mSwitchTo = new ToolStripMenuItem("Switch to...", null, TreeReposDoubleClick);
                ToolStripMenuItem mSetDefault = new ToolStripMenuItem("Set Default to...", null, MenuSetDefaultRepoToClick);
                ToolStripMenuItem mCommand = new ToolStripMenuItem("Command Prompt...", null, MenuViewCommandClick);

                mNew.Tag = String.Empty;
                mClone.Tag = repo;

                if (repo == App.Repos.Current) mSwitchTo.Enabled = false;
                if (repo == App.Repos.Default) mSetDefault.Enabled = false;

                string repoName = repo.Path.Replace('\\', '/').Split('/').Last();
                mSwitchTo.Text = "Switch to " + repoName;
                mSetDefault.Text = "Set Default to " + repoName;

                var items = new List<ToolStripItem> {
                    mNew, mScan, mEdit, mDelete, mClone,
                    new ToolStripSeparator(),
                    mSwitchTo, mSetDefault, mCommand
                };

                if (isRepoInProject)
                {
                    items.Add(new ToolStripSeparator());
                    items.Add(new ToolStripMenuItem("Remove from Project", null, MenuRemoveFromProjectClick));
                }

                items.Add(new ToolStripSeparator());
                items.Add(mNewProject);
                items.Add(new ToolStripSeparator());
                items.Add(mRefresh);

                return new ToolStripItemCollection(owner, items.ToArray());
            }

            // Empty space or no selection
            return new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mScan,
                new ToolStripSeparator(),
                mNewProject,
                new ToolStripSeparator(),
                mRefresh
            });
        }

        #endregion

        #region Tree mode drag-drop

        private void TreeReposItemDrag(object sender, ItemDragEventArgs e)
        {
            // TreeViewEx.OnItemDrag ensures the dragged node is in the selection.
            // Pass the selected nodes list as drag data for multi-drag support.
            if (treeRepos.SelectedNodes.Count > 0)
                DoDragDrop(treeRepos.SelectedNodes, DragDropEffects.Move);
        }

        private void TreeReposDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<TreeNode>)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (ValidGitRepos(data).Count > 0)
                    e.Effect = DragDropEffects.Copy;
                else if (data != null && data.Length == 1 && ClassUtils.DirStat(data[0]) == ClassUtils.DirStatType.Nongit)
                    e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TreeReposDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(List<TreeNode>)))
                return;

            Point pt = treeRepos.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeRepos.GetNodeAt(pt);
            List<TreeNode> dragNodes = (List<TreeNode>)e.Data.GetData(typeof(List<TreeNode>));

            if (targetNode == null || dragNodes.Contains(targetNode))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // Prevent dropping a project onto its own children
            foreach (TreeNode dragNode in dragNodes)
            {
                if (dragNode.Tag is ClassProject && IsDescendantOf(targetNode, dragNode))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }

            e.Effect = DragDropEffects.Move;

            // Auto-scroll near edges
            if (pt.Y < 15 && targetNode.PrevVisibleNode != null)
                targetNode.PrevVisibleNode.EnsureVisible();
            if (pt.Y > treeRepos.Height - 15 && targetNode.NextVisibleNode != null)
                targetNode.NextVisibleNode.EnsureVisible();
        }

        private void TreeReposDragDrop(object sender, DragEventArgs e)
        {
            // External file drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                HandleExternalDrop(e);
                return;
            }

            if (!e.Data.GetDataPresent(typeof(List<TreeNode>))) return;

            List<TreeNode> dragNodes = (List<TreeNode>)e.Data.GetData(typeof(List<TreeNode>));
            Point pt = treeRepos.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeRepos.GetNodeAt(pt);

            if (targetNode == null || dragNodes.Count == 0) return;
            if (dragNodes.Contains(targetNode)) return;

            var layout = App.Repos.ProjectLayout;

            // Determine drop position relative to target
            Rectangle bounds = targetNode.Bounds;
            int thirdHeight = Math.Max(bounds.Height / 3, 4);
            bool isTopZone = (pt.Y - bounds.Top) < thirdHeight;
            bool isBottomZone = (bounds.Bottom - pt.Y) < thirdHeight;
            bool isCenterZone = !isTopZone && !isBottomZone;

            // Process each dragged node. For multi-drag, only repo nodes are moved;
            // project nodes are only moved when dragged alone (single selection).
            // When inserting after a target (isBottomZone), reverse iteration order so that
            // the last item is inserted first, preserving the original selection order.
            IEnumerable<TreeNode> orderedNodes = isBottomZone ? Enumerable.Reverse(dragNodes) : (IEnumerable<TreeNode>)dragNodes;
            foreach (TreeNode dragNode in orderedNodes)
            {
                if (dragNode.Tag is ClassRepo)
                {
                    ClassRepo dragRepo = (ClassRepo)dragNode.Tag;
                    DropRepoNode(layout, dragRepo, targetNode, isTopZone, isBottomZone, isCenterZone);
                }
                else if (dragNode.Tag is ClassProject && dragNodes.Count == 1)
                {
                    DropProjectNode(layout, (ClassProject)dragNode.Tag, targetNode, isBottomZone);
                }
            }

            TreeRefresh();
        }

        /// <summary>
        /// Handle dropping a single repo node onto a target
        /// </summary>
        private void DropRepoNode(ClassProjectLayout layout, ClassRepo dragRepo,
            TreeNode targetNode, bool isTopZone, bool isBottomZone, bool isCenterZone)
        {
            if (targetNode.Tag is ClassProject && isCenterZone)
            {
                // Repo dropped ON center of a project node -> add to project at end
                layout.RemoveRepoFromProject(dragRepo.Path);
                layout.RootOrder.Remove("R:" + dragRepo.Path);
                layout.AddRepoToProject(((ClassProject)targetNode.Tag).Name, dragRepo.Path);
            }
            else if (targetNode.Tag is ClassProject && (isTopZone || isBottomZone))
            {
                // Repo dropped on top/bottom edge of project node -> reorder at root level
                layout.RemoveRepoFromProject(dragRepo.Path);
                layout.RootOrder.Remove("R:" + dragRepo.Path);
                string targetEntry = "P:" + ((ClassProject)targetNode.Tag).Name;
                layout.InsertAtRootLevel("R:" + dragRepo.Path, targetEntry, isBottomZone);
            }
            else if (targetNode.Tag is ClassRepo)
            {
                ClassRepo targetRepo = (ClassRepo)targetNode.Tag;

                if (targetNode.Parent != null && targetNode.Parent.Tag is ClassProject)
                {
                    // Target repo is inside a project -> move drag repo into that project
                    ClassProject targetProject = (ClassProject)targetNode.Parent.Tag;
                    int targetIndex = targetProject.RepoPaths.IndexOf(targetRepo.Path);
                    if (!isTopZone) targetIndex++;
                    layout.InsertRepoInProject(targetProject.Name, dragRepo.Path, targetIndex);
                }
                else
                {
                    // Target repo is at root level -> reorder at root level
                    layout.RemoveRepoFromProject(dragRepo.Path);
                    layout.RootOrder.Remove("R:" + dragRepo.Path);
                    string targetEntry = "R:" + targetRepo.Path;
                    layout.InsertAtRootLevel("R:" + dragRepo.Path, targetEntry, isBottomZone);
                }
            }
        }

        /// <summary>
        /// Handle dropping a project node onto a target (single drag only)
        /// </summary>
        private void DropProjectNode(ClassProjectLayout layout, ClassProject dragProject,
            TreeNode targetNode, bool isBottomZone)
        {
            if (targetNode.Level == 0)
            {
                string dragEntry = "P:" + dragProject.Name;
                string targetEntry;
                if (targetNode.Tag is ClassProject)
                    targetEntry = "P:" + ((ClassProject)targetNode.Tag).Name;
                else
                    targetEntry = "R:" + ((ClassRepo)targetNode.Tag).Path;

                layout.InsertAtRootLevel(dragEntry, targetEntry, isBottomZone);
            }
        }

        /// <summary>
        /// Handle external folder drops in tree mode (same logic as flat mode)
        /// </summary>
        private void HandleExternalDrop(DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<string> repos = ValidGitRepos(data);
                foreach (string repo in repos)
                {
                    App.PrintLogMessage("DropRepo: " + repo, MessageType.Debug);
                    AddNewRepo(repo, repos.Count == 1);
                }
            }
            else if (e.Effect == DragDropEffects.Link)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (data != null && data.Length == 1)
                {
                    string repo = data[0];
                    App.PrintLogMessage("DropRepo: " + repo, MessageType.Debug);
                    string root = NewRepoWizard(null, null, repo);
                    if (!string.IsNullOrEmpty(root))
                        AddNewRepo(root, true);
                }
            }
        }

        /// <summary>
        /// Check if 'node' is a descendant of 'ancestor'
        /// </summary>
        private bool IsDescendantOf(TreeNode node, TreeNode ancestor)
        {
            TreeNode current = node;
            while (current != null)
            {
                if (current == ancestor) return true;
                current = current.Parent;
            }
            return false;
        }

        #endregion

        #region Project menu handlers

        private void MenuNewProjectClick(object sender, EventArgs e)
        {
            string name = FormProjectName.GetName("New Project", "");
            if (name == null) return;

            if (!App.Repos.ProjectLayout.AddProject(name))
            {
                MessageBox.Show("A project with this name already exists.", "New Project",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReposRefresh();
        }

        private void MenuRenameProjectClick(object sender, EventArgs e)
        {
            ClassProject project = treeRepos.SelectedNode != null ? treeRepos.SelectedNode.Tag as ClassProject : null;
            if (project == null) return;

            string name = FormProjectName.GetName("Rename Project", project.Name);
            if (name == null || name == project.Name) return;

            if (!App.Repos.ProjectLayout.RenameProject(project.Name, name))
            {
                MessageBox.Show("A project with this name already exists.", "Rename Project",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReposRefresh();
        }

        private void MenuDeleteProjectClick(object sender, EventArgs e)
        {
            ClassProject project = treeRepos.SelectedNode != null ? treeRepos.SelectedNode.Tag as ClassProject : null;
            if (project == null) return;

            App.Repos.ProjectLayout.DeleteProject(project.Name);
            ReposRefresh();
        }

        private void MenuRemoveFromProjectClick(object sender, EventArgs e)
        {
            ClassRepo repo = GetSelectedRepo();
            if (repo == null) return;

            App.Repos.ProjectLayout.RemoveRepoFromProject(repo.Path);
            ReposRefresh();
        }

        #endregion

        #region Shared helpers (used by both modes)

        /// <summary>
        /// Return the selected repo object or null
        /// </summary>
        private ClassRepo GetSelectedRepo()
        {
            if (UseFlatMode)
            {
                if (listRepos.SelectedIndices.Count == 1)
                    return (ClassRepo)listRepos.Items[listRepos.SelectedIndices[0]].Tag;
                return null;
            }
            else
            {
                if (treeRepos.SelectedNode == null) return null;
                return treeRepos.SelectedNode.Tag as ClassRepo;
            }
        }

        /// <summary>
        /// Returns a list of repos which are selected.
        /// In tree mode, selecting a project returns all repos in that project.
        /// </summary>
        public List<ClassRepo> GetSelectedRepos()
        {
            var repos = new List<ClassRepo>();
            if (UseFlatMode)
            {
                foreach (int index in listRepos.SelectedIndices)
                {
                    ListViewItem li = listRepos.Items[index];
                    ClassRepo r = li.Tag as ClassRepo;
                    repos.Add(r);
                }
            }
            else
            {
                foreach (TreeNode node in treeRepos.SelectedNodes)
                {
                    if (node.Tag is ClassRepo)
                    {
                        ClassRepo r = (ClassRepo)node.Tag;
                        if (!repos.Contains(r))
                            repos.Add(r);
                    }
                    // Ctrl+click on project deep-selects children via TreeViewEx,
                    // so the child repo nodes will already be in SelectedNodes
                }
            }
            return repos;
        }

        /// <summary>
        /// Builds and returns a context menu (public entry point used by FormMain)
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
        {
            if (UseFlatMode)
                return GetFlatContextMenu(owner);
            else
                return GetTreeContextMenu(owner);
        }

        /// <summary>
        /// Restore a set of selected repos in the currently active view
        /// </summary>
        private void RestoreSelection(List<ClassRepo> repos)
        {
            if (repos.Count == 0) return;

            if (UseFlatMode)
            {
                foreach (ListViewItem li in listRepos.Items)
                {
                    ClassRepo r = li.Tag as ClassRepo;
                    if (r != null && repos.Contains(r))
                        li.Selected = true;
                }
            }
            else
            {
                var nodesToSelect = new List<TreeNode>();
                CollectRepoNodes(treeRepos.Nodes, repos, nodesToSelect);
                if (nodesToSelect.Count > 0)
                    treeRepos.SelectedNodes = nodesToSelect;
            }
        }

        /// <summary>
        /// Recursively find tree nodes matching the given repos
        /// </summary>
        private void CollectRepoNodes(TreeNodeCollection nodes, List<ClassRepo> repos, List<TreeNode> result)
        {
            foreach (TreeNode node in nodes)
            {
                ClassRepo r = node.Tag as ClassRepo;
                if (r != null && repos.Contains(r))
                    result.Add(node);
                if (node.Nodes.Count > 0)
                    CollectRepoNodes(node.Nodes, repos, result);
            }
        }

        /// <summary>
        /// Create a list of valid git repos from the array of potential repo paths
        /// Input data can be null but the output list may only be empty (not null)
        /// </summary>
        private List<string> ValidGitRepos(string[] data)
        {
            List<string> repoList = new List<string>();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (ClassUtils.DirStat(data[i]) == ClassUtils.DirStatType.Git)
                        repoList.Add(data[i]);
                }
            }
            return repoList;
        }

        /// <summary>
        /// Create or clone a new git repository.
        /// If the Tag field is non-empty, it contains the Repo to clone.
        /// </summary>
        private void MenuNewRepoClick(object sender, EventArgs e)
        {
            // If we need to clone a repo, set the cloning parameters within the Step1 form
            ClassRepo repoToClone = ((ToolStripDropDownItem)sender).Tag as ClassRepo;
            string root = NewRepoWizard(repoToClone, null, null);
            if (!string.IsNullOrEmpty(root))
                AddNewRepo(root, true);
        }

        /// <summary>
        /// This internal function adds a new repo
        /// After the repo has been added to the list, the repo edit dialog will open to let the user
        /// have a chance to modify its settings. This behavior can be disabled by setting openEdit to false.
        /// </summary>
        private void AddNewRepo(string path, bool openEdit)
        {
            try
            {
                // Simply add the new repo. This method will throw exceptions if something's not right.
                ClassRepo repo = App.Repos.Add(path);

                // Switch to the new repo and do a global refresh
                App.Repos.SetCurrent(repo);
                App.DoRefresh();

                // Open the Edit Repo dialog since the user may want to fill in user name and email, at least
                if (openEdit)
                    MenuRepoEditClick(null, null);
            }
            catch (Exception ex)
            {
                App.PrintLogMessage("Unable to add repo: " + ex.Message, MessageType.Error);
                App.PrintStatusMessage(ex.Message, MessageType.Error);
            }
        }

        /// <summary>
        /// Global static function that executes a new repo wizard
        /// If successful, returns the path to the new local repo
        /// If failed, returns null
        /// </summary>
        public static string NewRepoWizard(ClassRepo repoToClone, ClassRepo repoRemote, string folder)
        {
            FormNewRepoStep1 newRepoStep1 = new FormNewRepoStep1();
            FormNewRepoStep2 newRepoStep2 = new FormNewRepoStep2();

            if (repoToClone != null)
            {
                newRepoStep1.Type = "local";
                newRepoStep1.Local = repoToClone.ToString();
            }
            if (repoRemote != null)
            {
                List<string> remotes = repoRemote.Remotes.GetRemoteNames();
                if (remotes.Count > 0)
                {
                    newRepoStep1.Type = "remote";
                    ClassRemotes.Remote remote = repoRemote.Remotes.Get(remotes[0]);
                    newRepoStep1.SetRemote(remote);
                }
                else
                    newRepoStep2.Destination = repoRemote.Path;
            }
            bool skipStep1 = !string.IsNullOrEmpty(folder);
            newRepoStep2.Destination = folder ?? "";

        BackToStep1:
            if (skipStep1 || newRepoStep1.ShowDialog() == DialogResult.OK)
            {
                switch (newRepoStep1.Type)
                {
                    case "empty":
                        newRepoStep2.ProjectName = "";
                        newRepoStep2.CheckTargetDirEmpty = false;
                        break;
                    case "local":
                        string[] parts = newRepoStep1.Local.Split(new char[] { '\\', '/' });
                        if (parts.Length > 1)
                            newRepoStep2.ProjectName = parts[parts.Length - 1];
                        newRepoStep2.Destination = "";
                        newRepoStep2.CheckTargetDirEmpty = true;
                        break;
                    case "remote":
                        ClassRemotes.Remote r = newRepoStep1.Remote;
                        string remoteUrl = r.UrlFetch ?? "";

                        ClassUrl.Url parsedUrl = ClassUrl.Parse(remoteUrl);
                        if (!parsedUrl.Ok)
                        {
                            MessageBox.Show("The remote URL specification is invalid:\n\n" + remoteUrl +
                                "\n\nPlease enter a valid git remote URL.",
                                "Invalid Remote URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto BackToStep1;
                        }

                        newRepoStep2.ProjectName = parsedUrl.Name;

                        string dest = newRepoStep2.Destination ?? "";
                        if (!string.IsNullOrEmpty(dest) && !string.IsNullOrEmpty(newRepoStep2.ProjectName))
                        {
                            parts = dest.Split(new char[] { '\\', '/' });
                            if (parts.Length > 1 && String.Compare(parts[parts.Length - 1], newRepoStep2.ProjectName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                newRepoStep2.ProjectName = parts[parts.Length - 1];
                                try
                                {
                                    DirectoryInfo parent = Directory.GetParent(dest);
                                    if (parent != null)
                                        newRepoStep2.Destination = parent.FullName;
                                }
                                catch (Exception) { }
                            }
                        }
                        newRepoStep2.CheckTargetDirEmpty = true;
                        break;
                }

            BackToStep2:
                DialogResult result = newRepoStep2.ShowDialog();

                skipStep1 = false;
                if (result == DialogResult.Retry)
                    goto BackToStep1;

                if (result == DialogResult.OK)
                {
                    string type = newRepoStep1.Type;
                    string root = newRepoStep2.Destination;
                    string branch = newRepoStep2.InitBranchName;
                    bool isBranch = !string.IsNullOrEmpty(branch);
                    string extra = newRepoStep2.Extra;
                    bool isBare = newRepoStep2.IsBare;

                    try
                    {
                        string init = "";
                        ClassUtils.AddEnvar("PASSWORD", "");
                        switch (type)
                        {
                            case "empty":
                                init = "init \"" + root + "\"" + (isBranch ? " -b " + branch : "") + (isBare ? " --bare --shared=all " : " ") + extra;
                                break;

                            case "local":
                                init = "clone " + newRepoStep1.Local + " \"" + root + "\"" + (isBare ? " --bare --shared " : " ") + extra;
                                break;

                            case "remote":
                                ClassRemotes.Remote r = newRepoStep1.Remote;
                                init = "clone --progress -v --origin " + r.Name + " " + r.UrlFetch + " \"" + root + "\"" + (isBare ? " --bare --shared " : " ") + extra;
                                ClassUtils.AddEnvar("PASSWORD", r.Password);
                                break;
                        }

                        Directory.SetCurrentDirectory(App.AppHome);
                        if (ClassGit.Run(init).Success() == false)
                            goto BackToStep2;

                        return root;
                    }
                    catch (ClassException ex)
                    {
                        if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                            return string.Empty;
                    }
                    goto BackToStep2;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Scan the file system and find Git repositories to add to the workspace
        /// </summary>
        private void MenuScanRepoClick(object sender, EventArgs e)
        {
            FormNewRepoScan formScan = new FormNewRepoScan();
            if (formScan.ShowDialog() == DialogResult.OK)
            {
                List<string> dirs = formScan.GetList();
                if (dirs.Count > 0)
                {
                    FormNewRepoScanAdd formAdd = new FormNewRepoScanAdd(dirs);
                    formAdd.ShowDialog();
                    App.DoRefresh();
                }
            }
        }

        /// <summary>
        /// Edit a selected repository specification
        /// </summary>
        private void MenuRepoEditClick(object sender, EventArgs e)
        {
            ClassRepo repo = GetSelectedRepo();
            FormRepoEdit repoEdit = new FormRepoEdit(repo);
            if (repoEdit.ShowDialog() == DialogResult.OK)
                App.DoRefresh();
        }

        /// <summary>
        /// Delete selected repository and optionally more files (if a single repo is selected)
        /// If multiple repos are selected, simply remove them from our list.
        /// </summary>
        private void MenuDeleteRepoClick(object sender, EventArgs e)
        {
            if (UseFlatMode && listRepos.SelectedItems.Count > 1)
            {
                // Remove every selected repo from the list
                foreach (int index in listRepos.SelectedIndices)
                {
                    ListViewItem li = listRepos.Items[index];
                    ClassRepo r = li.Tag as ClassRepo;
                    App.Repos.Delete(r);
                }
                MenuRefreshClick(null, null);           // Heavy refresh
            }
            else
            {
                // Single selected repo offers more deletion choices...
                ClassRepo repo = GetSelectedRepo();
                if (repo == null) return;
                // The actual file deletion is implemented in FormDeleteRepo form class:
                FormDeleteRepo deleteRepo = new FormDeleteRepo(repo.Path);
                if (deleteRepo.ShowDialog() == DialogResult.OK)
                {
                    App.Repos.Delete(repo);
                    MenuRefreshClick(null, null);       // Heavy refresh
                }
            }
        }

        /// <summary>
        /// Set the default repo to the one selected. The default repo is automatically
        /// selected after program loads.
        /// </summary>
        private void MenuSetDefaultRepoToClick(object sender, EventArgs e)
        {
            App.Repos.Default = GetSelectedRepo();
            ReposRefresh();
        }

        /// <summary>
        /// Open a command prompt at the root directory of a selected repo,
        /// not necessarily the current repo
        /// </summary>
        private void MenuViewCommandClick(object sender, EventArgs e)
        {
            ClassRepo repo = GetSelectedRepo();
            if (repo != null)
                ClassUtils.CommandPromptHere(repo.Path);
        }

        /// <summary>
        /// Full refresh of the workspace repos.
        /// Every repo in the list is checked and invalid ones are removed from the list.
        /// </summary>
        private void MenuRefreshClick(object sender, EventArgs e)
        {
            // Do a global repo refresh, this may be a heavy operation when the number of repos is large
            App.StatusBusy(true);
            App.Repos.InitAll();
            App.StatusBusy(false);
            App.DoRefresh();
        }

        #endregion
    }
}
