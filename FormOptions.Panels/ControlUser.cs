using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormOptions_Panels
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
            textBoxUserName.Text = ClassConfig.Get("user.name");
            textBoxUserEmail.Text = ClassConfig.Get("user.email");

            // Add the dirty (modified) value changed helper
            textBoxUserName.TextChanged += ControlDirtyHelper.ControlDirty;
            textBoxUserEmail.TextChanged += ControlDirtyHelper.ControlDirty;
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            if (textBoxUserName.Tag != null)
            {
                ClassConfig.Set("user.name", textBoxUserName.Text);
                textBoxUserName.Tag = null;
            }

            if (textBoxUserEmail.Tag != null)
            {
                ClassConfig.Set("user.email", textBoxUserEmail.Text);
                textBoxUserEmail.Tag = null;
            }
        }
    }
}
