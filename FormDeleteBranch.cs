using System;
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
        private readonly ClassBranches branches;

        /// <summary>
        /// Default branch name to select when switching
        /// </summary>
        private readonly string defaultBranch;

        /// <summary>
        /// Constructor for the Delete Branch operation.
        /// It is given a name of the branch to select by default.
        /// </summary>
        public FormDeleteBranch(string defaultBranch)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            branches = App.Repos.Current.Branches;
            this.defaultBranch = defaultBranch;

            // Initialize local branches as the default view.
            // This assignment will call "RadioButton1CheckedChanged" since
            // we initialized this radio button to not-checked in the designer.
            radioLocalBranch.Checked = true;

            // Restore the state of the Force delete option
            checkForce.Checked = Properties.Settings.Default.ForceDeleteBranch;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormDeleteBranchFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
            Properties.Settings.Default.ForceDeleteBranch = checkForce.Checked;
        }

        /// <summary>
        /// Button clicked to delete a selected branch.
        /// This method is called only if a branch has been selected (otherwise the Delete button was disabled)
        /// </summary>
        private void DeleteClick(object sender, EventArgs e)
        {
            string cmd;
            string selectedBranch = listBranches.SelectedItem.ToString();

            // Depending on the branch (local or remote), use a different way to delete it
            if (radioLocalBranch.Checked)
                cmd = String.Format("branch {0} {1}", checkForce.Checked ? "-D" : "-d", selectedBranch);
            else
            {
                // Remote branch
                string repoName = selectedBranch.Split('/')[0];
                string branchName = selectedBranch.Split('/')[1];
                if (checkTracking.Checked)
                    cmd = String.Format("branch -rd {0}", branchName); // Remove a reference to a remote branch
                else
                    cmd = String.Format("push {0} :{1}", repoName, branchName);
            }

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
                        foreach (var branch in branches.Local)
                            listBranches.Items.Add(branch);
                        checkForce.Enabled = true;
                        checkTracking.Enabled = false;
                        break;
                    case "Remote":
                        foreach (var branch in branches.Remote)
                            listBranches.Items.Add(branch);
                        checkForce.Enabled = false;
                        checkTracking.Enabled = true;
                        break;
                }
                // Select the default branch if it is present on this list
                int defaultIndex = listBranches.Items.IndexOf(defaultBranch);
                btDelete.Enabled = false;
                if (defaultIndex >= 0)
                    listBranches.SelectedIndex = defaultIndex;
            }
        }

        /// <summary>
        /// Enable the Delete button if something has been selected
        /// </summary>
        private void ListBranchesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBranches.SelectedItem != null)
                btDelete.Enabled = true;
        }
    }
}
