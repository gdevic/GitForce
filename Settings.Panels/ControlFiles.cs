using System;
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
            CheckBoxScanTabsCheckedChanged(null, null);
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
            checkBoxScanTabs.Checked = Properties.Settings.Default.WarnOnTabs;
            textBoxScanExt.Text = Properties.Settings.Default.WarnOnTabsExt;

            // Add the dirty (modified) value changed helper
            checkBoxIgnoreCase.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            checkBoxRefreshOnChange.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            checkBoxReaddOnChange.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            checkBoxScanTabs.CheckStateChanged += ControlDirtyHelper.ControlDirty;
            textBoxScanExt.TextChanged += ControlDirtyHelper.ControlDirty;
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
            Properties.Settings.Default.WarnOnTabs = checkBoxScanTabs.Checked;
            Properties.Settings.Default.WarnOnTabsExt = textBoxScanExt.Text;

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

        /// <summary>
        /// The edit text field to specify which files to check is dependent on the warn on tabs checkbox change
        /// </summary>
        private void CheckBoxScanTabsCheckedChanged(object sender, EventArgs e)
        {
            textBoxScanExt.Enabled = checkBoxScanTabs.Checked;
        }
    }
}
