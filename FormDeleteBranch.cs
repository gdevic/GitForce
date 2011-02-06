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
    public partial class FormDeleteBranch : Form
    {
        /// <summary>
        /// Cache the origin lists of branches, so we fetch them only once,
        /// and also are able to populate them dynamically as the user selects a radio button
        /// </summary>
        private string[] _localBranches;
        private string[] _remoteBranches;

        /// <summary>
        /// Singular branch name selected among options for local or remote
        /// </summary>
        private string _branchName;

        public FormDeleteBranch()
        {
            InitializeComponent();

            // Initialize local branches as the default selection
            _localBranches = App.Repos.Current.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            FormNewBranch.ListAdd(ref listBranches, ref _localBranches);
        }

        /// <summary>
        /// Button clicked to delete a selected branch
        /// </summary>
        private void DeleteClick(object sender, EventArgs e)
        {
            StringBuilder cmd = new StringBuilder("branch ");

            cmd.Append(checkForce.Checked ? "-D " : "-d ");

            if (radioButton2.Checked)
                cmd.Append("-r ");

            cmd.Append(_branchName);

            // Execute the final branch command
            App.Repos.Current.Run(cmd.ToString());
        }

        /// <summary>
        /// Called on a change of radio button selection for the branch selection
        /// </summary>
        private void RadioButton1CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                switch (rb.Tag.ToString())
                {
                    case "Local":
                        FormNewBranch.ListAdd(ref listBranches, ref _localBranches);
                        break;
                    case "Remote":
                        if (_remoteBranches == null)
                            _remoteBranches = App.Repos.Current.Run("branch -r").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        FormNewBranch.ListAdd(ref listBranches, ref _remoteBranches);
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
            _branchName = listBranches.SelectedItem.ToString();
            btDelete.Enabled = true;
        }
    }
}
