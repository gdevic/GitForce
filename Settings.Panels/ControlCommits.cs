using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlCommits : UserControl, IUserSettings
    {
        public ControlCommits()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            textBoxLast.Text = Properties.Settings.Default.commitsRetrieveLast.ToString();
            rbRetrieveAll.Checked = Properties.Settings.Default.commitsRetrieveAll;
            rbRetrieveLast.Checked = !rbRetrieveAll.Checked;
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            int last;
            if (int.TryParse(textBoxLast.Text, out last) == false)
                last = 100;
            Properties.Settings.Default.commitsRetrieveLast = last;
            Properties.Settings.Default.commitsRetrieveAll = rbRetrieveAll.Checked;
        }

        /// <summary>
        /// Retrieve number of changes radio button changed
        /// </summary>
        private void RbRetrieveAllCheckedChanged(object sender, EventArgs e)
        {
            textBoxLast.ReadOnly = rbRetrieveAll.Checked;
        }
    }
}
