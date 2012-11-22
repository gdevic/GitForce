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
    public partial class ControlOptions : UserControl, IUserSettings
    {
        public ControlOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            checkBoxTabs.Checked = Properties.Settings.Default.ShowTabsOnRightPane;

            // Add the dirty (modified) value changed helper
            checkBoxTabs.CheckStateChanged += ControlDirtyHelper.ControlDirty;
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
            Properties.Settings.Default.ShowTabsOnRightPane = checkBoxTabs.Checked;

            if (checkBoxTabs.Tag != null)
                App.MainForm.UpdateRightPaneTabs();
        }
    }
}
