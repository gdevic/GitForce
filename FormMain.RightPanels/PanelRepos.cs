using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormMain_RightPanels
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
        /// Fill in the list of repositories in the GUI listbox from the repos list
        /// </summary>
        private void ReposRefresh()
        {
            listRepos.BeginUpdate();
            listRepos.Items.Clear();
            foreach (ClassRepo r in App.Repos.Repos)
            {
                ListViewItem li = new ListViewItem(r.Root);
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, ClassConfig.Get("user.name", r)));
                li.SubItems.Add(new ListViewItem.ListViewSubItem(li, ClassConfig.Get("user.email", r)));
                
                // List items' tags are actual pointers to repos' individual objects
                li.Tag = r;
                li.ImageIndex = (int)RepoIcons.RepoIdle;

                // Select the 'current' repository in the list
                if (r == App.Repos.Current)
                {
                    li.Selected = true;
                    li.ImageIndex |= 2; // Bit [1] is current
                }

                if (r.Root == App.Repos.GetDefault())
                    li.ImageIndex |= 1; // Bit [0] is default

                listRepos.Items.Add(li);
            }

            // Make columns auto-adjust to fit the width of the largest item
            foreach (ColumnHeader l in listRepos.Columns) l.Width = -2;
            listRepos.EndUpdate();
        }

        /// <summary>
        /// Return the selected repo object or null if no repo is selected (no repos in the list)
        /// </summary>
        public ClassRepo GetSelectedRepo()
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
            // Menus are set in this order:
            //  [0]  New...        -> always open a dialog
            //  [1]  Edit...       -> opens a dialog to edit selected repo
            //  [2]  Delete...     -> opens a dialog to confirm deletion of a selected repo
            //  [3]  -----------
            //  [4]  Switch to     -> switch to selected repo
            //  [5]  Set Default   -> set default repo to selected repo

            ToolStripMenuItem mNew = new ToolStripMenuItem("New...", null, MenuNewRepoClick);
            ToolStripMenuItem mEdit = new ToolStripMenuItem("Edit...", null, MenuRepoEditClick);
            ToolStripMenuItem mDelete = new ToolStripMenuItem("Delete...", null, MenuDeleteRepoClick);
            ToolStripMenuItem mSwitchTo = new ToolStripMenuItem("Switch to...", null, ListReposDoubleClick);
            ToolStripMenuItem mSetDefault = new ToolStripMenuItem("Set Default to...", null, MenuSetDefaultRepoToClick);

            ToolStripItemCollection menu = new ToolStripItemCollection(owner, new ToolStripItem[] {
                mNew, mEdit, mDelete,
                new ToolStripSeparator(),
                mSwitchTo, mSetDefault
            });

            if (GetSelectedRepo() == null)
                mEdit.Enabled = mDelete.Enabled = mSwitchTo.Enabled = mSetDefault.Enabled = false;

            return menu;
        }

        /// <summary>
        /// Create or clone a new git repository
        /// </summary>
        private static void MenuNewRepoClick(object sender, EventArgs e)
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
                    ClassRepo repo = null;

                    try
                    {
                        string init;
                        switch (type)
                        {
                            case "empty":
                                init = "init " + root + (isBare ? " --bare --shared=all " : " ") + extra;
                                repo = App.Repos.Add(root);
                                repo.Run(init);
                                break;

                            case "local":
                                init = "clone " + newRepoStep1.Local + " " + root + (isBare ? " --bare --shared " : " ") + extra;
                                App.Git.Run(init, null, root);
                                repo = App.Repos.Add(root);
                                break;

                            case "remote":
                                ClassRemotes.Remote r = newRepoStep1.Remote;

                                init = "clone --origin " + r.Name + " " + r.UrlFetch + " " + root + (isBare ? " --bare --shared " : " ") + extra;
                                App.Git.Run(init, null, root, r.Password);
                                repo = App.Repos.Add(root);
                                break;
                        }

                        // Switch the view mode to Local File View and Local Pending Changelists
                        App.MainForm.ResetViews();

                        // Finally, switch to the new repo which will cause a global refresh
                        App.Repos.SetCurrent(repo);
                    }
                    catch (ClassException ex)
                    {
                        MessageBox.Show(ex.Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
        /// Delete selected repository and optionally more files
        /// </summary>
        private void MenuDeleteRepoClick(object sender, EventArgs e)
        {
            ClassRepo repo = GetSelectedRepo();
            FormDeleteRepo deleteRepo = new FormDeleteRepo(repo.Root);
            if (deleteRepo.ShowDialog() == DialogResult.OK)
                App.Repos.Delete(repo);
        }

        /// <summary>
        /// Set the default repo to the one selected. The default repo is automatically
        /// selected after program loads.
        /// </summary>
        private void MenuSetDefaultRepoToClick(object sender, EventArgs e)
        {
            App.Repos.SetDefault(GetSelectedRepo());
            ReposRefresh();
        }
    }
}
