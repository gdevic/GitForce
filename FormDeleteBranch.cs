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
        private string[] localBranches = null;
        private string[] remoteBranches = null;

        /// <summary>
        /// Singular branch name selected among options for local or remote
        /// </summary>
        private string branchName = null;

        public FormDeleteBranch()
        {
            InitializeComponent();

            // Initialize local branches as the default selection
            localBranches = App.Git.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            FormNewBranch.listAdd(ref listBranches, ref localBranches);
        }

        /// <summary>
        /// Button clicked to delete a selected branch
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            StringBuilder cmd = new StringBuilder("branch ");

            cmd.Append(checkForce.Checked ? "-D " : "-d ");

            if (radioButton2.Checked)
                cmd.Append("-r ");

            cmd.Append(branchName);

            // Execute the final branch command
            App.Git.Run(cmd.ToString());
        }

        /// <summary>
        /// Called on a change of radio button selection for the branch selection
        /// </summary>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                switch (rb.Tag.ToString())
                {
                    case "Local":
                        FormNewBranch.listAdd(ref listBranches, ref localBranches);
                        break;
                    case "Remote":
                        if (remoteBranches == null)
                            remoteBranches = App.Git.Run("branch -r").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        FormNewBranch.listAdd(ref listBranches, ref remoteBranches);
                        break;
                }
            }
        }

        /// <summary>
        /// Store the selected item name of a local or remote branch into the branch
        /// tracking variable.
        /// </summary>
        private void listBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            branchName = listBranches.SelectedItem.ToString();
            btDelete.Enabled = true;
        }
    }
}
