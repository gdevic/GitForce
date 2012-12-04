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
    /// Form to create a new branch from a list of options.
    /// </summary>
    public partial class FormNewBranch : Form
    {
        /// <summary>
        /// Shortcut variable to the main app's current repo
        /// </summary>
        private readonly ClassBranches branches;

        /// <summary>
        /// Singular branch origin selected among various options with a radio button
        /// </summary>
        private string origin = "";

        public FormNewBranch()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            branches = App.Repos.Current.Branches;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormNewBranchFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Called on a change of radio button selection for the branch origin
        /// </summary>
        private void RadioBranchSourceCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked == false)
            {
                switch (rb.Tag.ToString())
                {
                    case "SHA1":
                        textSHA1.Enabled = false;
                        break;
                    default:
                        listBranches.Enabled = false;
                        listBranches.BackColor = SystemColors.Control;
                        break;
                }
                origin = null;
            }
            else
            {
                switch (rb.Tag.ToString())
                {
                    case "Head":
                        break;
                    case "SHA1":
                        textSHA1.Enabled = true;
                        break;
                    case "Local":
                        listBranches.Items.Clear();
                        foreach (var branch in branches.Local)
                            listBranches.Items.Add(branch);
                        listBranches.Enabled = true;
                        listBranches.BackColor = SystemColors.Window;
                        break;
                    case "Remote":
                        listBranches.Items.Clear();
                        foreach (var branch in branches.Remote)
                            listBranches.Items.Add(branch);
                        listBranches.Enabled = true;
                        listBranches.BackColor = SystemColors.Window;
                        break;
                    case "Tag":
                        listBranches.Items.Clear();
                        ExecResult result = App.Repos.Current.Run("tag");
                        if (result.Success())
                        {
                            string[] tags = result.stdout.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var tag in tags)
                                listBranches.Items.Add(tag);
                            listBranches.Enabled = true;
                        }
                        listBranches.BackColor = SystemColors.Window;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Button clicked to create a new branch and exit the dialog
        /// </summary>
        private void BtCreateClick(object sender, EventArgs e)
        {
            string name = textBranchName.Text.Trim();

            string cmd = String.Format("branch {0} {1}", name, origin);
            ExecResult result = App.Repos.Current.RunCmd(cmd);
            if (result.Success())
            {
                // Check out the branch if needed
                if (checkCheckOut.Checked)
                {
                    result = App.Repos.Current.RunCmd("checkout " + name);
                    if (result.Success() == false)
                        MessageBox.Show(result.stderr, "Error checking out a branch", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show(result.stderr, "Error creating a branch", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Store the selected item name of a local, remote branch or tag into the origin
        /// tracking variable.
        /// </summary>
        private void ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            origin = listBranches.SelectedItem.ToString();
        }

        /// <summary>
        /// Store the SHA1 textual value as it is being typed, into the origin
        /// tracking variable
        /// </summary>
        private void TextSha1TextChanged(object sender, EventArgs e)
        {
            origin = textSHA1.Text;
        }

        /// <summary>
        /// Limit the character set that can be used to specify the branch name
        /// or SHA1 key (somewhat loosely in order to reuse the function)
        /// </summary>
        private void TextBranchNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && e.KeyChar != '-' && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void TextBranchNameKeyUp(object sender, KeyEventArgs e)
        {
            // Enable the Create button if we have the branch name
            btCreate.Enabled = textBranchName.Text.Length > 0;
        }
    }
}
