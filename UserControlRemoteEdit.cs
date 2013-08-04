using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    public partial class RemoteEdit : UserControl
    {
        /// <summary>
        /// Local cache of the repo that we are editing
        /// </summary>
        private ClassRepo _repo;

        /// <summary>
        /// Currently selected remote repo in our editing listbox
        /// </summary>
        private ClassRemotes.Remote _current;

        public RemoteEdit()
        {
            InitializeComponent();
        }

        public void SetRepo(ClassRepo repo)
        {
            _repo = repo;
            ListRefresh();

            // Select the first item, if there is any
            if (listRemotes.Items.Count > 0)
                listRemotes.SelectedIndex = 0;
        }

        private void ListRefresh()
        {
            _repo.Remotes.Refresh(_repo);

            // Populate the list box
            userControlRemoteDisplay.Clear();
            listRemotes.BeginUpdate();
            listRemotes.Items.Clear();

            // Get the list of names from remotes class and iteratively add them to the listbox
            foreach (var r in _repo.Remotes.GetListNames())
                listRemotes.Items.Add(r);

            listRemotes.EndUpdate();
        }

        private void ListRemotesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRemotes.SelectedIndex >= 0)
            {
                string name = listRemotes.SelectedItem.ToString();
                _current = _repo.Remotes.Get(name);
                userControlRemoteDisplay.Set(_current);

                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = true;
            }
            else
                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = false;
        }

        /// <summary>
        /// Add new remote repository
        /// </summary>
        private void BtAddClick(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote = new ClassRemotes.Remote();
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Add, remote);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                remote = remoteAddEdit.Get();
                ExecResult result = _repo.Run("remote add " + remote.Name + " " + remote.UrlFetch);
                if (result.Success())
                {
                    _repo.Remotes.SetPassword(remote.Name, remote.Password);
                    _repo.Remotes.SetPushCmd(remote.Name, remote.PushCmd);
                    SetRepo(_repo);
                }
                else
                {
                    MessageBox.Show("Git is unable to add this remote repository.", "Add remote repository", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    App.PrintStatusMessage(result.stderr, MessageType.Error);
                }
            }
        }

        /// <summary>
        /// Edit selected remote repository
        /// </summary>
        private void BtEditClick(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote;
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Edit, _current);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                // Get new values and start comparing what changed
                remote = remoteAddEdit.Get();

                if (remote.UrlFetch != _current.UrlFetch)
                {
                    // Change the fetch URL
                    _repo.Run("remote set-url " + remote.Name + " " + remote.UrlFetch);

                    // However, this will also change the push URL, so reset it
                    if (_current.UrlPush != "")
                        _repo.Run("remote set-url --push " + remote.Name + " " + _current.UrlPush);
                }

                if (remote.UrlPush != _current.UrlPush)
                {
                    // Change the push URL
                    _repo.Run("remote set-url --push " + remote.Name + " " + remote.UrlPush);
                }

                if (remote.Password != _current.Password)
                {
                    // Change the password
                    _repo.Remotes.SetPassword(remote.Name, remote.Password);
                }

                if (remote.PushCmd != _current.PushCmd)
                {
                    // Change the push command
                    _repo.Remotes.SetPushCmd(remote.Name, remote.PushCmd);
                }

                SetRepo(_repo);
            }
        }

        /// <summary>
        /// Rename selected remote repository
        /// </summary>
        private void BtRenameClick(object sender, EventArgs e)
        {
            ClassRemotes.Remote remote;
            FormRemoteAddEdit remoteAddEdit = new FormRemoteAddEdit();
            remoteAddEdit.Prepare(FormRemoteAddEdit.Function.Rename, _current);

            if (remoteAddEdit.ShowDialog() == DialogResult.OK)
            {
                // Get new name and change it
                remote = remoteAddEdit.Get();
                if (remote.Name != _current.Name)
                {
                    _repo.Run("remote rename " + _current.Name + " " + remote.Name);
                    SetRepo(_repo);
                }
            }
        }

        /// <summary>
        /// Delete selected remote repository
        /// </summary>
        private void BtDeleteClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete reference to a remote repository '" + _current.Name + "'\nProceed?", "Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _repo.Run("remote rm " + _current.Name);

                btEdit.Enabled = btRename.Enabled = btDelete.Enabled = false;

                // Refresh and select the next item, if any
                SetRepo(_repo);
            }
        }
    }
}
