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
        private string[] _localBranches;
        private string[] _remoteBranches;
        private string[] _tags;

        /// <summary>
        /// Singular branch origin selected among various options with a radio button
        /// </summary>
        private string _origin;

        public FormNewBranch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a set of branches or tags to a given listBox, trimming as necessary
        /// to arrive at a workable name. This function also enables the list box.
        /// This function is also called from Delete branch form, so it is static.
        /// </summary>
        /// <param name="listBranches"></param>
        /// <param name="list"></param>
        public static void ListAdd(ref ListBox listBranches, ref string[] list)
        {
            listBranches.Items.Clear();
            foreach (string name in
                list.Select(s => s.Replace(" remotes/", "").Replace("*", " ").Trim()).
                Select(name => name.Split((" ").ToCharArray())[0]))
            {
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
                _origin = null;
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
                        if (_localBranches == null)
                            _localBranches = App.Repos.Current.Run("branch").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        ListAdd(ref listBranches, ref _localBranches);
                        break;
                    case "Remote":
                        if (_remoteBranches == null)
                            _remoteBranches = App.Repos.Current.Run("branch -r").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        ListAdd(ref listBranches, ref _remoteBranches);
                        break;
                    case "Tag":
                        if (_tags == null)
                            _tags = App.Repos.Current.Run("tag").Split(("\n").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        ListAdd(ref listBranches, ref _tags);
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

            StringBuilder cmd = new StringBuilder();

            cmd.Append(checkCheckOut.Checked ? "checkout -b " : "branch ");

            cmd.Append(name);

            if (_origin != null)
                cmd.Append(" " + _origin);

            // Execute the final branch command
            App.Repos.Current.Run(cmd.ToString());
        }

        /// <summary>
        /// Store the selected item name of a local, remote branch or tag into the origin
        /// tracking variable.
        /// </summary>
        private void ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _origin = listBranches.SelectedItem.ToString(); 
        }

        /// <summary>
        /// Store the SHA1 textual value as it is being typed, into the origin
        /// tracking variable
        /// </summary>
        private void TextSha1TextChanged(object sender, EventArgs e)
        {
            _origin = textSHA1.Text;
        }

        /// <summary>
        /// Limit the character set that can be used to specify the branch name
        /// or SHA1 key (somewhat loosely in order to reuse the function)
        /// </summary>
        private static void TextBranchNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!='_' && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void TextBranchNameKeyUp(object sender, KeyEventArgs e)
        {
            // Enable the Create button if we have the branch name
            btCreate.Enabled = textBranchName.Text.Length > 0;
        }
    }
}
