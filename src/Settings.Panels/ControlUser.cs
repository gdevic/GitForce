using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlUser : UserControl, IUserSettings
    {
        public ControlUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            textBoxUserName.Text = ClassConfig.GetGlobal("user.name");
            textBoxUserEmail.Text = ClassConfig.GetGlobal("user.email");

            // Add the dirty (modified) value changed helper
            textBoxUserName.TextChanged += ControlDirtyHelper.ControlDirty;
            textBoxUserEmail.TextChanged += ControlDirtyHelper.ControlDirty;
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
            if (textBoxUserName.Tag != null)
            {
                ClassConfig.SetGlobal("user.name", textBoxUserName.Text.Trim());
                textBoxUserName.Tag = null;
            }

            if (textBoxUserEmail.Tag != null)
            {
                ClassConfig.SetGlobal("user.email", textBoxUserEmail.Text.Trim());
                textBoxUserEmail.Tag = null;
            }
        }
    }
}
