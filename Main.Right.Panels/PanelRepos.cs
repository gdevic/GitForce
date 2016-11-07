using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GitForce.Main.Right.Panels
{
    public partial class PanelRepos : UserControl
    {
        enum RepoIcons { RepoIdle, RepoDefault, RepoCurrent, RepoBoth }

        public PanelRepos()
        {
            InitializeComponent();

            App.Refresh += ReposRefresh;
        }

        /// <summary>
        /// Fill in the list of repositories.
        /// </summary>
        public void ReposRefresh()
        {
            listRepos.BeginUpdate();
            listRepos.Items.Clear();
            foreach (ClassRepo r in App.Repos.Repos)
            {
                ListViewItem li = new ListViewItem(r.Root);
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

            // Adjust the header columns
            foreach (ColumnHeader l in listRepos.Columns)
            {
                string values = Properties.Settings.Default.ReposColumnWidths;
                int[] columns = values.Split(',').Select(Int32.Parse).ToArray();
                // Either set the column width from the user settings, or
                // make columns auto-adjust to fit the width of the largest item
                if (Properties.Settings.Default.ReposColumnWidths != null
                   && columns[l.Index] > 0 )
                    l.Width = columns[l.Index];
                else
                    l.Width = -2;
            }
            listRepos.EndUpdate();
        }

        /// <summary>
        /// Save columns' widths every time they change. This will happen when the
        /// form loads for the first time and then every time a user drags and resizes a column
        /// </summary>
        private void ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            // Store new width only when there are items on the list. This check also prevents
            // the code from storing initial loaded columns when the form is created
            if (listRepos.Items.Count > 0)
            {
                // WAR: We used to save this int[] type variable directly but in the settings it would embed "<?xml version..."
                // text in the middle of a file which would break the Linux mono code
                string values = Properties.Settings.Default.ReposColumnWidths;
                int[] columns = values.Split(',').Select(Int32.Parse).ToArray();
                columns[e.ColumnIndex] = listRepos.Columns[e.ColumnIndex].Width;
                values = String.Join(",", columns.Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray());
                Properties.Settings.Default.ReposColumnWidths = values;
            }
        }

        /// <summary>
        /// Return the selected repo object or null if no repo is selected (no repos in the list)
        /// </summary>
        private ClassRepo GetSelectedRepo()
        {
            if (listRepos.SelectedIndices.Count == 1)
                return (ClassRepo)listRepos.Items[listRepos.SelectedIndices[0]].Tag;
            return null;
        }

        /// <summary>
        /// Returns a list of repos which are selected (not necessarily the "current" or "default")
        /// </summary>
        public List<ClassRepo> GetSelectedRepos()
        {
            var repos = new List<ClassRepo>();
            foreach (int index in listRepos.SelectedIndices)
            {
                ListViewItem li = listRepos.Items[index];
                ClassRepo r = li.Tag as ClassRepo;
                repos.Add(r);
            }
            return repos;
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
        /// Right-mouse button opens a popup with the context menu
        /// </summary>
        private void ListReposMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Build the context menu to be shown
                contextMenu.Items.Clear();
                contextMenu.Items.AddRange(GetContextMenu(contextMenu));
            }
        }

        /// <summary>
        /// Builds ands returns a context menu for branches
        /// </summary>
        public ToolStripItemCollection GetContextMenu(ToolStrip owner)
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

            // Disable some menus depending on few rules
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
            // We can delete a number of repos
            if (listRepos.SelectedIndices.Count > 1)
            {
                mDelete.Enabled = true;
                mCommand.Enabled = false;
                mClone.Enabled = false;
            }

            // Add the specific singular repo name
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
        /// Create or clone a new git repository.
        /// If the Tag field is non-empty, it contains the Repo to clone.
        /// </summary>
        private void MenuNewRepoClick(object sender, EventArgs e)
        {
            // If we need to clone a repo, set the cloning parameters within the Step1 form
            ClassRepo repoToClone = ((ToolStripDropDownItem)sender).Tag as ClassRepo;
            string root = NewRepoWizard(repoToClone, null);
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
        public static string NewRepoWizard(ClassRepo repoToClone, ClassRepo repoRemote)
        {
            FormNewRepoStep1 newRepoStep1 = new FormNewRepoStep1();
            FormNewRepoStep2 newRepoStep2 = new FormNewRepoStep2();

            // If the repo to clone parameter was given, build the close repo information
            if (repoToClone != null)
            {
                newRepoStep1.Type = "local";
                newRepoStep1.Local = repoToClone.ToString();
            }
            // If the remote repo parameter was given, build the remote repo information
            if (repoRemote != null)
            {
                // If the list of remotes contains at least one entry, use it
                List<string> remotes = repoRemote.Remotes.GetRemoteNames();
                if (remotes.Count > 0)
                {
                    newRepoStep1.Type = "remote";
                    ClassRemotes.Remote remote = repoRemote.Remotes.Get(remotes[0]);
                    newRepoStep1.SetRemote(remote);
                }
                else
                    newRepoStep2.Destination = repoRemote.Root;
            }

        BackToStep1:
            if (newRepoStep1.ShowDialog() == DialogResult.OK)
            {
                // Depending on the type of the source, establish that:
                //  For clone operations:
                //  - The final directory has to exist and be empty
                //  - New repo name will be suggested based on the source project names
                switch (newRepoStep1.Type)
                {
                    case "empty":
                        newRepoStep2.ProjectName = "";
                        newRepoStep2.CheckTargetDirEmpty = false;
                        break;
                    case "local":
                        // Find the project name from the cloned path (the last part of the path)
                        string[] parts = newRepoStep1.Local.Split(new char[] { '\\', '/' });
                        if (parts.Length > 1)
                            newRepoStep2.ProjectName = parts[parts.Length - 1];
                        newRepoStep2.Destination = "";
                        newRepoStep2.CheckTargetDirEmpty = true;
                        break;
                    case "remote":
                        ClassRemotes.Remote r = newRepoStep1.Remote;
                        string remoteProjectName = r.UrlFetch;
                        // Extract the project name from the remote Url specification
                        parts = remoteProjectName.Split(new char[] { '.', '\\', '/', ':' });
                        if (parts.Length > 1 && parts[parts.Length - 1] == "git")
                            newRepoStep2.ProjectName = parts[parts.Length - 2];

                        // If the project name is equal to the last part of the path, use it for the project name instead
                        // and trim the path. This is done to (1) propagate possible upper-cased letters in the
                        // path and (2) to fix the cases where we have given repoRemote with a full path which included
                        // a redundant project name.
                        // Example: root:         c:\Projects\Arduino    =>   c:\Projects
                        //          project name: arduino                =>   Arduino
                        parts = newRepoStep2.Destination.Split(new char[] { '\\', '/' });
                        if (parts.Length > 1 && String.Compare(parts[parts.Length - 1], newRepoStep2.ProjectName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            newRepoStep2.ProjectName = parts[parts.Length - 1];
                            newRepoStep2.Destination = Directory.GetParent(newRepoStep2.Destination).FullName;
                        }
                        newRepoStep2.CheckTargetDirEmpty = true;
                        break;
                }

            BackToStep2:
                DialogResult result = newRepoStep2.ShowDialog();

                // Clicking on the <<Prev button will return "Retry" result, so we loop back to the first form...
                if (result == DialogResult.Retry)
                    goto BackToStep1;

                if (result == DialogResult.OK)
                {
                    string type = newRepoStep1.Type;
                    string root = newRepoStep2.Destination;
                    string extra = newRepoStep2.Extra;
                    bool isBare = newRepoStep2.IsBare;

                    try
                    {
                        string init = "";
                        switch (type)
                        {
                            case "empty":
                                init = "init \"" + root + "\"" + (isBare ? " --bare --shared=all " : " ") + extra;
                                break;

                            case "local":
                                init = "clone " + newRepoStep1.Local + " \"" + root + "\"" + (isBare ? " --bare --shared " : " ") + extra;
                                break;

                            case "remote":
                                ClassRemotes.Remote r = newRepoStep1.Remote;
                                init = "clone --progress -v --origin " + r.Name + " " + r.UrlFetch + " \"" + root + "\"" + (isBare ? " --bare --shared " : " ") + extra;

                                // Add HTTPS password for the next execute of a clone operation
                                ClassUtils.AddEnvar("PASSWORD", r.Password);
                                break;
                        }

                        // Get out of the way (so the git can remove directory if the clone operation fails)
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
            if( listRepos.SelectedItems.Count>1)
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
                // The actual file deletion is implemented in FormDeleteRepo form class:
                FormDeleteRepo deleteRepo = new FormDeleteRepo(repo.Root);
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
            ClassUtils.CommandPromptHere(repo.Root);
        }

        /// <summary>
        /// Full refresh of the workspace repos.
        /// Every repo in the list is checked and invalid ones are removed from the list.
        /// </summary>
        private void MenuRefreshClick(object sender, EventArgs e)
        {
            // Do a global repo refresh, this may be a heavy operation when the number of repos is large
            App.StatusBusy(true);
            App.Repos.Refresh();
            App.StatusBusy(false);
            App.DoRefresh();
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
            // The user is dropping another list view item; he is reordering the list
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
                string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (ValidGitRepos(data).Count > 0)
                {
                    // We use Copy effect for outside drop (see TreeViewEx limitation)
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        /// <summary>
        /// Create a list of valid git repos from the array of potential repo paths
        /// Input data can be null but the output list may only be empty (not null)
        /// </summary>
        private List<string> ValidGitRepos(string[] data)
        {
            List<string> repos = new List<string>();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (ClassUtils.DirStat(data[i]) == ClassUtils.DirStatType.Git)
                        repos.Add(data[i]);
                }
            }
            return repos;
        }

        /// <summary>
        /// The only purpose of this handler is to fix a Linux listview issue where
        /// the header is sometimes not visible when a tab is switched to
        /// </summary>
        private void ListReposVisibleChanged(object sender, EventArgs e)
        {
            if (!ClassUtils.IsMono()) return; // Linux/Mono fixup only
            if (!Visible) return; // Only on becoming visible
            // Adjust the header columns
            listRepos.BeginUpdate();
            foreach (ColumnHeader l in listRepos.Columns)
            {
                string values = Properties.Settings.Default.ReposColumnWidths;
                int[] columns = values.Split(',').Select(Int32.Parse).ToArray();
                // Either set the column width from the user settings, or
                // make columns auto-adjust to fit the width of the largest item
                if (Properties.Settings.Default.ReposColumnWidths != null
                   && columns[l.Index] > 0)
                    l.Width = columns[l.Index];
                else
                    l.Width = -2;
            }
            listRepos.EndUpdate();
        }
    }
}
