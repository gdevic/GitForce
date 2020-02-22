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
            checkBoxWarnMultipleInstances.Checked = Properties.Settings.Default.WarnMultipleInstances;
            checkBoxWarnIfAdmin.Checked = Properties.Settings.Default.WarnIfAdmin;
            checkBoxAutoCloseGitOnSuccess.Checked = Properties.Settings.Default.AutoCloseGitOnSuccess;

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
            Properties.Settings.Default.WarnMultipleInstances = checkBoxWarnMultipleInstances.Checked;
            Properties.Settings.Default.WarnIfAdmin = checkBoxWarnIfAdmin.Checked;
            Properties.Settings.Default.AutoCloseGitOnSuccess = checkBoxAutoCloseGitOnSuccess.Checked;

            if (checkBoxTabs.Tag != null)
                App.MainForm.UpdateRightPaneTabsShowState();
        }
    }
}
