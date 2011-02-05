using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class RemoteEdit : UserControl
    {
        /// <summary>
        /// Local cache of the repo that we are editing
        /// </summary>
        ClassRepo repo;

        /// <summary>
        /// Currently selected remote repo in our editing listbox
        /// </summary>
        private ClassRemotes.Remote current;

        public RemoteEdit()
        {
            InitializeComponent();
        }

        public void setRepo(ClassRepo _repo)
        {
            repo = _repo;
            listRefresh();

            // Select the first item, if there is any
            if (listRemotes.Items.Count > 0)
                listRemotes.SelectedIndex = 0;
        }

        private void listRefresh()
        {
            repo.remotes.Refresh(repo);

            // Populate the list box
            userControlRemoteDisplay.Clear();
            listRemotes.BeginUpdate();
            listRemotes.Items.Clear();

            // Get the list of names from remotes class and iteratively add them to the listbox
            foreach (var r in repo.remotes.GetListNames())
                listRemotes.Items.Add(r);

            listRemotes.EndUpdate();
        }

        private void listRemotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRemotes.SelectedIndex >= 0)
            {
                string name = listRemotes.SelectedItem.ToString();
                current = repo.remotes.Get(name);
                userControlRemoteDisplay.Set(current);

                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = true;
            }
            else
                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = false;
        }

        /// <summary>
        /// Add new remote repository
        /// </summary>
        private void btAdd_Click(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote = new ClassRemotes.Remote();
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Add, remote);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                remote = remoteAddEdit.Get();
                repo.Run("remote add " + remote.name + " " + remote.urlFetch);
                repo.remotes.SetPassword(remote.name, remote.password);

                setRepo(repo);
            }
        }

        /// <summary>
        /// Edit selected remote repository
        /// </summary>
        private void btEdit_Click(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote;
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Edit, current);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                // Get new values and start comparing what changed
                remote = remoteAddEdit.Get();

                if (remote.urlFetch != current.urlFetch)
                {
                    // Change the fetch URL
                    repo.Run("remote set-url " + remote.name + " " + remote.urlFetch);

                    // However, this will also change the push URL, so reset it
                    if (current.urlPush != "")
                        repo.Run("remote set-url --push " + remote.name + " " + current.urlPush);
                }

                if (remote.urlPush != current.urlPush)
                {
                    // Change the push URL
                    repo.Run("remote set-url --push " + remote.name + " " + remote.urlPush);
                }

                if (remote.password != current.password)
                {
                    // Change the password
                    repo.remotes.SetPassword(remote.name, remote.password);
                }
                setRepo(repo);
            }
        }

        /// <summary>
        /// Rename selected remote repository
        /// </summary>
        private void btRename_Click(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote;
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Rename, current);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                // Get new name and change it
                remote = remoteAddEdit.Get();
                if (remote.name != current.name)
                {
                    repo.Run("remote rename " + current.name + " " + remote.name);
                    setRepo(repo);
                }
            }
        }

        /// <summary>
        /// Delete selected remote repository
        /// </summary>
        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete reference to a remote repository '" + current.name + "'\nProceed?", "Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                repo.Run("remote rm " + current.name);

                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = false;

                // Refresh and select the next item, if any
                setRepo(repo);
            }
        }
    }
}
