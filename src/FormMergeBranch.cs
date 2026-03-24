using System;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Form to merge a branch.
    /// </summary>
    public partial class FormMergeBranch : Form
    {
        public FormMergeBranch()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            ClassBranches branches = App.Repos.Current.Branches;

            labelCurrentBranchName.Text = "Current branch is \"" + branches.Current + "\"";

            // Add all available branches to the list of branches to merge
            foreach (var branch in branches.Local)
                listBranches.Items.Add(branch);

            foreach (var branch in branches.Remote)
                listBranches.Items.Add(branch);

            listBranches.Items.RemoveAt(listBranches.Items.IndexOf(branches.Current));

            if (listBranches.Items.Count > 0)
            {
                listBranches.SelectedIndex = 0;
                btMerge.Enabled = true;
            }
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormMergeBranchFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Button is clicked to perform a branch merge.
        /// </summary>
        private void BtMergeClick(object sender, EventArgs e)
        {
            string cmd = "merge " + listBranches.SelectedItem;

            App.Repos.Current.RunCmd(cmd);
        }
    }
}
