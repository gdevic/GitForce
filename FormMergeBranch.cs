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
    public partial class FormMergeBranch : Form
    {
        public FormMergeBranch(ClassBranches branches)
        {
            InitializeComponent();

            MergeStyle.SetStyle(Properties.Settings.Default.mergeStyle);

            labelCurrentBranchName.Text = "Current branch is \"" + branches.Current + "\"";
            listBranches.Items.AddRange(branches.Local.ToArray());
            listBranches.Items.AddRange(branches.Remote.ToArray());
            listBranches.Items.RemoveAt(listBranches.Items.IndexOf(branches.Current));
            if (listBranches.Items.Count > 0)
            {
                listBranches.SelectedIndex = 0;
                btMerge.Enabled = true;
            }
        }

        public bool DoMerge(string branch)
        {
            const bool ret = false;
            if (!string.IsNullOrEmpty(branch))
            {


            }
            return ret;
        }

        private void BtMergeClick(object sender, EventArgs e)
        {
            DoMerge(listBranches.SelectedItem.ToString());
        }
    }
}
