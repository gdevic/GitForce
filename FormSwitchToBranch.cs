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
    public partial class FormSwitchToBranch : Form
    {
        private string[] localBranches = null;

        /// <summary>
        /// Singular branch name that is selected to be switched to
        /// </summary>
        private string branchName = null;

        public FormSwitchToBranch()
        {
            InitializeComponent();

            // Initialize the list of local branches to switch to
            localBranches = App.Git.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            FormNewBranch.listAdd(ref listBranches, ref localBranches);
        }

        /// <summary>
        /// Button is clicked to switch to a (local) branch
        /// </summary>
        private void SwitchBranch_Click(object sender, EventArgs e)
        {
            StringBuilder cmd = new StringBuilder("checkout ");

            cmd.Append(branchName.ToString());

            // Execute the final branch command
            App.Git.Run(cmd.ToString());
        }

        /// <summary>
        /// Store the selected branch
        /// </summary>
        private void listBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            branchName = listBranches.SelectedItem.ToString();
            btSwitch.Enabled = true;
        }
    }
}
