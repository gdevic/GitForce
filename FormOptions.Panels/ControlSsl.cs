using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace git4win.FormOptions_Panels
{
    public partial class ControlSsl : UserControl, IUserSettings
    {
        public ControlSsl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            checkBoxLeavePageant.Checked = Properties.Settings.Default.leavePageant;

            // Add the dirty (modified) value changed helper
            checkBoxLeavePageant.CheckStateChanged += ControlDirtyHelper.ControlDirty;
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.leavePageant = checkBoxLeavePageant.Checked;
        }

        /// <summary>
        /// Open the dialog to manage SSH keys
        /// </summary>
        private void btSshClick(object sender, EventArgs e)
        {
            FormSSH formSsh = new FormSSH();
            formSsh.ShowDialog();
        }
    }
}
