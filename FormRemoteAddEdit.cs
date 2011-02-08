using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class FormRemoteAddEdit : Form
    {
        /// <summary>
        /// Specifies one of 3 main functions that this form supports
        /// </summary>
        public enum Function { Add, Edit, Rename };

        public FormRemoteAddEdit()
        {
            InitializeComponent();
        }

        public void Prepare(Function fn, ClassRemotes.Remote remote)
        {
            remoteDisplay.Clear();
            remoteDisplay.AnyTextChanged += SomeTextChanged;

            // Do things differently basen on whether we are using this
            // form to add a new remote repo, rename it or we are editing
            // an existing remote repo

            switch (fn)
            {
                case Function.Add:
                    remoteDisplay.Enable(true, true);
                    remote.Name = "origin";
                    remote.PushCmd = "";
                    remoteDisplay.Set(remote);
                    Text = "Add a new remote repository";
                    btOK.Enabled = false;
                    break;
                case Function.Edit:
                    Text = "Edit remote repository '" + remote.Name + "'";
                    remoteDisplay.Enable(false, true);
                    remoteDisplay.Set(remote);
                    break;
                case Function.Rename:
                    Text = "Rename remote repository '" + remote.Name + "'";
                    remoteDisplay.Enable(true, false);
                    remoteDisplay.Set(remote);
                    break;
            }
        }

        /// <summary>
        /// Return the content of the remote repo specification structure
        /// </summary>
        /// <returns>Remote repo values</returns>
        public ClassRemotes.Remote Get()
        {
            return remoteDisplay.Get();
        }

        /// <summary>
        /// Callback function called when text in the remote repo editing
        /// has changed. Enable or disable OK button based on some simple
        /// checks.
        /// </summary>
        private void SomeTextChanged(bool valid)
        {
            btOK.Enabled = valid;
        }
    }
}
