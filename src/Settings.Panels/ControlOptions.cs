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
            checkBoxWarnOnDetachedHead.Checked = Properties.Settings.Default.WarnOnDetachedHead;
            // The system theme follows a Windows setting
            if (ClassUtils.IsMono())
                comboBoxTheme.Items.Remove(ClassTheme.ModeSystem);
            comboBoxTheme.Left = labelTheme.Right + 6;
            comboBoxTheme.SelectedItem = Properties.Settings.Default.Theme;
            if (comboBoxTheme.SelectedIndex < 0)
                comboBoxTheme.SelectedItem = ClassTheme.ModeLight;

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
            Properties.Settings.Default.WarnOnDetachedHead = checkBoxWarnOnDetachedHead.Checked;

            // The theme is read once, at startup
            string theme = comboBoxTheme.SelectedItem as string;
            if (theme != null)
                Properties.Settings.Default.Theme = theme;

            if (checkBoxTabs.Tag != null)
                App.MainForm.UpdateRightPaneTabsShowState();
        }
    }
}
