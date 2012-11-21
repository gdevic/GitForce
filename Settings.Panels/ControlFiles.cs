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
    public partial class ControlFiles : UserControl, IUserSettings
    {
        public ControlFiles()
        {
            InitializeComponent();

            // Update visuals on dependent checkbox as described below
            CheckBoxRefreshOnChangeCheckedChanged(null, null);
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            string value = ClassConfig.GetGlobal("core.ignorecase");
            checkBoxIgnoreCase.Checked = (value.ToLower() == "true") ? true : false;
            checkBoxShowDotGit.Checked = Properties.Settings.Default.ShowDotGitFolders;
            checkBoxDeepScan.Checked = Properties.Settings.Default.RepoDeepScan;
            checkBoxRefreshOnChange.Checked = Properties.Settings.Default.RefreshOnChange;
            checkBoxReaddOnChange.Checked = Properties.Settings.Default.ReaddOnChange;

            // Add the dirty (modified) value changed helper
            checkBoxIgnoreCase.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            checkBoxRefreshOnChange.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            checkBoxReaddOnChange.CheckStateChanged += ControlDirtyHelper.ControlDirty;
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
            if (checkBoxIgnoreCase.Tag != null)
            {
                ClassConfig.SetGlobal("core.ignorecase", checkBoxIgnoreCase.Checked.ToString());
                checkBoxIgnoreCase.Tag = null;
            }

            Properties.Settings.Default.ShowDotGitFolders = checkBoxShowDotGit.Checked;
            Properties.Settings.Default.RepoDeepScan = checkBoxDeepScan.Checked;
            Properties.Settings.Default.RefreshOnChange = checkBoxRefreshOnChange.Checked;
            Properties.Settings.Default.ReaddOnChange = checkBoxReaddOnChange.Checked;

            // If the auto-refresh settings were changed, run the commits refresh to (de)arm the code
            if(checkBoxRefreshOnChange.Tag != null || checkBoxReaddOnChange.Tag != null)
            {
                App.MainForm.SelectiveRefresh(FormMain.SelectveRefreshFlags.Commits);
                checkBoxRefreshOnChange.Tag = checkBoxReaddOnChange.Tag = null;
            }
        }

        /// <summary>
        /// The option to re-add on change is dependent on refresh on change
        /// </summary>
        private void CheckBoxRefreshOnChangeCheckedChanged(object sender, EventArgs e)
        {
            checkBoxReaddOnChange.Enabled = checkBoxRefreshOnChange.Checked;
        }
    }
}
