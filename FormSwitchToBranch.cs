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
    /// Form to switch a branch to the one selected
    /// </summary>
    public partial class FormSwitchToBranch : Form
    {
        /// <summary>
        /// Singular branch name that is selected to be switched to
        /// </summary>
        private string _branchName;

        public FormSwitchToBranch()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            // Initialize the list of local branches to switch to
            foreach (var branch in App.Repos.Current.Branches.Local)
                listBranches.Items.Add(branch);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormSwitchToBranchFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Button is clicked to switch to a (local) branch
        /// </summary>
        private void SwitchBranchClick(object sender, EventArgs e)
        {
            // Execute the final branch command
            App.Repos.Current.Branches.SwitchTo(_branchName);
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
