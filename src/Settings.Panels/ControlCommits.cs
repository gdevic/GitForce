using System;
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
            numBoxLast.Value = Properties.Settings.Default.commitsRetrieveLast;
            rbRetrieveAll.Checked = Properties.Settings.Default.commitsRetrieveAll;
            rbRetrieveLast.Checked = !rbRetrieveAll.Checked;
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.commitsRetrieveLast = (int)numBoxLast.Value;
            Properties.Settings.Default.commitsRetrieveAll = rbRetrieveAll.Checked;
        }

        /// <summary>
        /// Retrieve number of changes radio button changed
        /// </summary>
        private void RbRetrieveAllCheckedChanged(object sender, EventArgs e)
        {
            numBoxLast.ReadOnly = rbRetrieveAll.Checked;
        }
    }
}
