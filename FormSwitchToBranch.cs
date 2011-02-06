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
        private readonly string[] _localBranches;

        /// <summary>
        /// Singular branch name that is selected to be switched to
        /// </summary>
        private string _branchName;

        public FormSwitchToBranch()
        {
            InitializeComponent();

            // Initialize the list of local branches to switch to
            _localBranches = App.Repos.Current.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            FormNewBranch.ListAdd(ref listBranches, ref _localBranches);
        }

        /// <summary>
        /// Button is clicked to switch to a (local) branch
        /// </summary>
        private void SwitchBranchClick(object sender, EventArgs e)
        {
            StringBuilder cmd = new StringBuilder("checkout ");

            cmd.Append(_branchName);

            // Execute the final branch command
            App.Repos.Current.Run(cmd.ToString());
        }

        /// <summary>
        /// Store the selected branch
        /// </summary>
        private void ListBranchesSelectedIndexChanged(object sender, EventArgs e)
        {
            _branchName = listBranches.SelectedItem.ToString();
            btSwitch.Enabled = true;
        }
    }
}
