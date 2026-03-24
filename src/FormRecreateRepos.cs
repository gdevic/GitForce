using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GitForce.Main.Right.Panels;

namespace GitForce
{
    /// <summary>
    /// Recreate missing repos and manage loaded repos before they are added to the
    /// main application's list of repos.
    /// </summary>
    public partial class FormRecreateRepos : Form
    {
        /// <summary>
        /// Local store for the list of repo we are managing
        /// Get and Set should not be called before this class has been instantiated
        /// </summary>
        public List<ClassRepo> Repos
        {
            // We keep the repos in the listbox's item's Tag field
            // Get method reads them from the listbox and returns a regular list of repos
            get
            {
                List<ClassRepo> repos = new List<ClassRepo>();
                foreach (ListViewItem item in list.Items)
                {
                    ClassRepo repo = item.Tag as ClassRepo;
                    repos.Add(repo);
                }
                return repos;
            }

            // We keep the repos in the listbox's item's Tag field
            // Set method stores repos in individual listbox' items
            set
            {
                // Build a list of mis-configured repos and show it on our listbox
                foreach (ClassRepo repo in value)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = repo;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, ""));
                    list.Items.Add(item);
                }
                RefreshView();
            }
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public FormRecreateRepos()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing, store the dialog geometry
        /// </summary>
        private void FormRecreateReposFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Keypress handler
        /// When the user presses F5, refresh the status of all listed folders
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                RefreshView();
                return true;            // Indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Check and refresh the view for each repo
        /// This method also enables the form's Close button if all repos are checked OK
        /// </summary>
        private void RefreshView()
        {
            list.Columns[1].Width = 75;
            bool enableClosing = true;
            foreach (ListViewItem item in list.Items)
            {
                ClassRepo repo = item.Tag as ClassRepo;
                item.Text = repo.Path;
                ClassUtils.DirStatType stat = ClassUtils.DirStat(repo.Path);
                switch (stat)
                {
                    case ClassUtils.DirStatType.Invalid:
                        item.SubItems[1].Text = "Non-existing";
                        enableClosing = false;
                        item.SubItems[0].ForeColor = Color.Red;
                        break;
                    case ClassUtils.DirStatType.Empty:
                        item.SubItems[1].Text = "Empty";
                        enableClosing = false;
                        break;
                    case ClassUtils.DirStatType.Git:
                        item.SubItems[1].Text = "OK";
                        item.SubItems[0].ForeColor = Color.DarkOliveGreen;
                        break;
                    case ClassUtils.DirStatType.Nongit:
                        item.SubItems[1].Text = "Non-empty";
                        enableClosing = false;
                        break;
                }
            }
            btClose.Enabled = enableClosing;
        }

        /// <summary>
        /// Resize the header subitems to keep up with the size change
        /// </summary>
        private void ListSizeChanged(object sender, EventArgs e)
        {
            list.Columns[0].Width = list.Width - list.Columns[1].Width - SystemInformation.VerticalScrollBarWidth - 5;
        }

        /// <summary>
        /// Selection of the repo items has changed
        /// Update button enables and common path fields correspondingly
        /// </summary>
        private void ListSelectedIndexChanged(object sender, EventArgs e)
        {
            // Delete button is enabled when one or more repos are selected
            btDelete.Enabled = list.SelectedItems.Count > 0;

            // Enable buttons that are defined to work when only one repo is selected
            btLocate.Enabled = btCreate.Enabled = list.SelectedItems.Count == 1;

            // Create repo should be disabled on a valid git repo
            if (btCreate.Enabled)
                btCreate.Enabled = ClassUtils.DirStat(list.SelectedItems[0].Text) != ClassUtils.DirStatType.Git;

            // Extract common path prefix of all selected repos - only if 2 or more repos are selected
            btBrowse.Enabled = list.SelectedItems.Count > 1;
            textRootPath.Text = "";
            if (list.SelectedItems.Count>1)
            {
                // Use the first path as the longest known so far
                List<String> commonPath = list.SelectedItems[0].Text.Split(Path.DirectorySeparatorChar).ToList();
                // Each successive path will make the "commonPath" shorter
                for (int i = 1; i < list.SelectedItems.Count; i++)
                {
                    String[] nextStr = list.SelectedItems[i].Text.Split(Path.DirectorySeparatorChar);
                    List<String> newCommon = new List<string>();
                    for (int j = 0; j < nextStr.Count() && j < commonPath.Count(); j++)
                    {
                        if (commonPath[j] == nextStr[j])
                            newCommon.Add(commonPath[j]);
                        else
                            break;
                    }
                    commonPath = newCommon;
                }
                textRootPath.Text = String.Join(Path.DirectorySeparatorChar.ToString(), commonPath.ToArray());
                btBrowse.Enabled = commonPath.Count > 0;
            }
        }

        /// <summary>
        /// User clicked on the Browse / common path button
        /// Change common parts of the repo paths for all selected repos. Open the directory finder
        /// and let the user select a new folder to use as a common root path
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            folder.Description = @"Select a new root path to substitute the common part of the paths of all selected repos:";
            folder.ShowNewFolderButton = true;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                // We are sure that 2 or more repos will be selected
                foreach (int index in list.SelectedIndices)
                {
                    ClassRepo repo = list.Items[index].Tag as ClassRepo;
                    repo.Path = folder.SelectedPath + repo.Path.Substring(textRootPath.Text.Length);
                    RefreshView();
                }
                // Disable Browse button so the user can't immediately repeat the operation since we
                // just changed all paths and would need a selection changed event to rebuild the textRootPath
                btBrowse.Enabled = false;
            }
        }

        /// <summary>
        /// User clicked on the Create button
        /// Run the new repo wizard to create/clone/pull that missing repo
        /// </summary>
        private void BtCreateClick(object sender, EventArgs e)
        {
            // We are sure that one and only one repo will be selected
            ClassRepo repo = list.SelectedItems[0].Tag as ClassRepo;

            string root = PanelRepos.NewRepoWizard(null, repo, null);
            if (!string.IsNullOrEmpty(root))
            {
                repo.Path = root;
                FormRepoEdit repoEdit = new FormRepoEdit(repo);
                repoEdit.ShowDialog();

                // Modify our list of repos to show the new path
                list.SelectedItems[0].Tag = repo;
                RefreshView();
            }
        }

        /// <summary>
        /// User clicked on the Locate button
        /// Open the directory finder and accept a new repo root. This method is called only
        /// when a single repo is being selected and the root of that repo will be changed.
        /// </summary>
        private void BtLocateClick(object sender, EventArgs e)
        {
            folder.Description = @"Select a folder where this git repository is located:";
            folder.ShowNewFolderButton = false;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                // We are sure that one and only one repo will be selected
                ClassRepo repo = list.SelectedItems[0].Tag as ClassRepo;
                repo.Path = folder.SelectedPath;
                RefreshView();
            }
        }

        /// <summary>
        /// User clicked on the Delete button
        /// Remove all selected repos from the list
        /// </summary>
        private void BtDeleteClick(object sender, EventArgs e)
        {
            while (list.SelectedItems.Count > 0)
                list.Items.Remove(list.SelectedItems[0]);
            RefreshView();
        }

        /// <summary>
        /// User clicked on the Help button, open the context help
        /// </summary>
        private void BtHelpClick(object sender, EventArgs e)
        {
            ClassHelp.Handler("Workspace");
        }
    }
}
