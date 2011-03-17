using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win.Main.Right.Panels
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
        private void ReposRefresh()
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

            // Make columns auto-adjust to fit the width of the largest item
            foreach (ColumnHeader l in listRepos.Columns) l.Width = -2;
            listRepos.EndUpdate();

            App.PrintStatusMessage("Number of repos: " + App.Repos);
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
        /// Double-clicking on a repository will switch to it.
        /// </summary>
        private void ListReposDoubleClick(object sender, EventArgs e)
        {
            App.Repos.SetCurrent(GetSelectedRepo());
            App.Refresh();
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
            ToolStripMenuItem mSwitchTo = new ToolStripMenuItem("Switch to...", null, ListReposDoubleClick);
            ToolStripMenuItem mSetDefault = new ToolStripMenuItem("Set Default to...", null, MenuSetDefaultRepoToClick);
            ToolStripMenuItem mRefresh = new ToolStripMenuItem("Refresh", null, MenuRefreshClick, Keys.F5);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mScan, mEdit, mDelete,
                new ToolStripSeparator(),
                mSwitchTo, mSetDefault,
                new ToolStripSeparator(),
                mRefresh
            });

            // Disable some menus depending on few rules
            ClassRepo repo = GetSelectedRepo();

            if (repo == null)
                mEdit.Enabled = mDelete.Enabled = mSwitchTo.Enabled = mSetDefault.Enabled = false;
            else
            {
                if (repo == App.Repos.Current)
                    mSwitchTo.Enabled = false;

                if (repo == App.Repos.Default)
                    mSetDefault.Enabled = false;
            }
            // We can delete a number of repos
            if (listRepos.SelectedIndices.Count > 1)
                mDelete.Enabled = true;

            // Add the specific singular repo name
            if (listRepos.SelectedIndices.Count == 1)
            {
                string repoName = listRepos.SelectedItems[0].Text.Replace('\\', '/').Split('/').Last();
                mSwitchTo.Text = "Switch to " + repoName;
                mSetDefault.Text = "Set Default to " + repoName;
            }

            return menu;
        }

        /// <summary>
        /// Create or clone a new git repository
        /// </summary>
        private void MenuNewRepoClick(object sender, EventArgs e)
        {
            FormNewRepoStep1 newRepoStep1 = new FormNewRepoStep1();
            FormNewRepoStep2 newRepoStep2 = new FormNewRepoStep2();

            BackToStep1:
            if (newRepoStep1.ShowDialog() == DialogResult.OK)
            {
                // Enforce target directory being empty for clone operations
                newRepoStep2.EnforceDirEmpty = newRepoStep1.Type != "empty";

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
                                init = "init " + root + (isBare ? " --bare --shared=all " : " ") + extra;
                                break;

                            case "local":
                                init = "clone " + newRepoStep1.Local + " " + root + (isBare ? " --bare --shared " : " ") + extra;
                                break;

                            case "remote":
                                ClassRemotes.Remote r = newRepoStep1.Remote;
                                init = "clone --progress -v --origin " + r.Name + " " + r.UrlFetch + " " + root + (isBare ? " --bare --shared " : " ") + extra;

                                // Add HTTPS password for the next execute of a clone operation
                                ClassExecute.AddEnvar("PASSWORD", r.Password);
                                break;
                        }

                        Directory.SetCurrentDirectory(root);
                        ClassGit.Run(init);
                        ClassRepo repo = App.Repos.Add(root);

                        // Switch the view mode to Local File View and Local Pending Changelists
                        App.MainForm.ResetViews();

                        // Finally, switch to the new repo and do a global refresh
                        App.Repos.SetCurrent(repo);
                        App.Refresh();
                    }
                    catch (ClassException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
                    App.Refresh();
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
                App.Refresh();
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
        /// Full refresh of the workspace repos.
        /// Every repo in the list is checked and invalid ones are removed from the list.
        /// </summary>
        private void MenuRefreshClick(object sender, EventArgs e)
        {
            // Do a global repo refresh, this may be a heavy operation when the number of repos is large
            App.StatusBusy(true);
            App.Repos.Refresh();
            App.StatusBusy(false);
            App.Refresh();
        }
    }
}
