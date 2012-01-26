using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Form to delete a selected branch
    /// </summary>
    public partial class FormDeleteBranch : Form
    {
        /// <summary>
        /// Shortcut variable to the main app's current repo
        /// </summary>
        private readonly ClassBranches _branches;

        /// <summary>
        /// Singular branch name selected among options for local or remote
        /// </summary>
        private string _branchName;

        public FormDeleteBranch()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            _branches = App.Repos.Current.Branches;

            // Initialize local branches as the default selection
            foreach (var branch in _branches.Local)
                listBranches.Items.Add(branch);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormDeleteBranchFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Button clicked to delete a selected branch
        /// </summary>
        private void DeleteClick(object sender, EventArgs e)
        {
            string cmd = String.Format("branch {0} {1} {2}",
                checkForce.Checked? "-D" : "-d",
                radioButton2.Checked? "-r": "",
                _branchName );

            // Execute the final branch command and if fail, show the dialog box asking to retry
            ExecResult result = App.Repos.Current.RunCmd(cmd);
            if (result.Success() == false)
                if (MessageBox.Show(result.stderr, "Error deleting branch", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    DialogResult = DialogResult.None;
        }

        /// <summary>
        /// Called on a change of radio button selection for the branch selection
        /// </summary>
        private void RadioButton1CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                listBranches.Items.Clear();

                switch (rb.Tag.ToString())
                {
                    case "Local":
                        foreach (var branch in _branches.Local)
                            listBranches.Items.Add(branch);
                        break;
                    case "Remote":
                        foreach (var branch in _branches.Remote)
                            listBranches.Items.Add(branch);
                        break;
                }
            }
        }

        /// <summary>
        /// Store the selected item name of a local or remote branch into the branch
        /// tracking variable.
        /// </summary>
        private void ListBranchesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBranches.SelectedItem != null)
            {
                _branchName = listBranches.SelectedItem.ToString();
                btDelete.Enabled = true;
            }
        }
    }
}
