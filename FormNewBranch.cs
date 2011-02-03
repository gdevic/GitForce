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
    public partial class FormNewBranch : Form
    {
        /// <summary>
        /// Cache the origin lists of branches and tags, so we fetch them only once,
        /// and also are able to populate them dynamically as the user selects a radio button
        /// </summary>
        private string[] localBranches = null;
        private string[] remoteBranches = null;
        private string[] tags = null;

        /// <summary>
        /// Singular branch origin selected among various options with a radio button
        /// </summary>
        private string origin = null;

        public FormNewBranch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a set of branches or tags to a given listBox, trimming as necessary
        /// to arrive at a workable name. This function also enables the list box.
        /// This function is also called from Delete branch form, so it is static.
        /// </summary>
        /// <param name="list"></param>
        public static void listAdd(ref ListBox listBranches, ref string[] list)
        {
            listBranches.Items.Clear();
            foreach (string s in list)
            {
                // Trim the spaces, current branch marker and 'remotes' keyword from the branch name
                string name = s.Replace(" remotes/", "").
                                Replace("*", " ").
                                Trim();

                // Trim a possible origin redirection ("branch -> origin/master")
                name = name.Split((" ").ToCharArray())[0];

                listBranches.Items.Add(name);
            }
            if (listBranches.Items.Count > 0)
                listBranches.SelectedItem = listBranches.Items[0];
            listBranches.Enabled = true;
            listBranches.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Called on a change of radio button selection for the branch origin
        /// </summary>
        private void radioBranchSource_CheckedChanged(object sender, EventArgs e)
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
                        if (localBranches == null)
                            localBranches = App.Git.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        listAdd(ref listBranches, ref localBranches);
                        break;
                    case "Remote":
                        if (remoteBranches == null)
                            remoteBranches = App.Git.Run("branch -r").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        listAdd(ref listBranches, ref remoteBranches);
                        break;
                    case "Tag":
                        if (tags == null)
                            tags = App.Git.Run("tag").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        listAdd(ref listBranches, ref tags);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Button clicked to create a new branch and exit the dialog
        /// </summary>
        private void btCreate_Click(object sender, EventArgs e)
        {
            string name = textBranchName.Text.ToString().Trim();

            StringBuilder cmd = new StringBuilder();

            if (checkCheckOut.Checked == true)
                cmd.Append("checkout -b ");
            else
                cmd.Append("branch ");

            cmd.Append(name);

            if (origin != null)
                cmd.Append(" " + origin);

            // Execute the final branch command
            App.Git.Run(cmd.ToString());
        }

        /// <summary>
        /// Store the selected item name of a local, remote branch or tag into the origin
        /// tracking variable.
        /// </summary>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            origin = listBranches.SelectedItem.ToString(); 
        }

        /// <summary>
        /// Store the SHA1 textual value as it is being typed, into the origin
        /// tracking variable
        /// </summary>
        private void textSHA1_TextChanged(object sender, EventArgs e)
        {
            origin = textSHA1.Text.ToString();
        }

        /// <summary>
        /// Limit the character set that can be used to specify the branch name
        /// or SHA1 key (somewhat loosely in order to reuse the function)
        /// </summary>
        private void textBranchName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!='_' && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void textBranchName_KeyUp(object sender, KeyEventArgs e)
        {
            // Enable the Create button if we have the branch name
            btCreate.Enabled = textBranchName.Text.Length > 0;
        }
    }
}
