using System;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlStatus : UserControl, IUserSettings
    {
        public ControlStatus()
        {
            InitializeComponent();

            // Update visuals on dependent checkbox as described below
            CheckBoxShowTimestampCheckedChanged(null, null);
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            checkBoxShowGitCommands.Checked = Properties.Settings.Default.logCommands;
            checkBoxShowTimestamp.Checked = Properties.Settings.Default.logTime;
            checkBoxUse24HourClock.Checked = Properties.Settings.Default.logTime24;
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
            Properties.Settings.Default.logCommands = checkBoxShowGitCommands.Checked;
            Properties.Settings.Default.logTime = checkBoxShowTimestamp.Checked;
            Properties.Settings.Default.logTime24 = checkBoxUse24HourClock.Checked;
        }

        /// <summary>
        /// The option to use 24-hour clock is dependent on showing the timestamp
        /// </summary>
        private void CheckBoxShowTimestampCheckedChanged(object sender, EventArgs e)
        {
            checkBoxUse24HourClock.Enabled = checkBoxShowTimestamp.Checked;
        }
    }
}
