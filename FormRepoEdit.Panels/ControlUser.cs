using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormRepoEdit_Panels
{
    public partial class ControlUser : UserControl, IRepoSettings
    {
        public ControlUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="options">All git global settings</param>
        public void Init(ClassRepo repo, string[] options)
        {
            textBoxUserName.Text = ClassConfig.Get("user.name", repo);
            textBoxUserEmail.Text = ClassConfig.Get("user.email", repo);

            // Add the dirty (modified) value changed helper
            textBoxUserName.TextChanged += ControlDirtyHelper.ControlDirty;
            textBoxUserEmail.TextChanged += ControlDirtyHelper.ControlDirty;
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
            if (textBoxUserName.Tag != null)
            {
                ClassConfig.Set("user.name", textBoxUserName.Text, repo);
                textBoxUserName.Tag = null;
            }

            if (textBoxUserEmail.Tag != null)
            {
                ClassConfig.Set("user.email", textBoxUserEmail.Text, repo);
                textBoxUserEmail.Tag = null;
            }
        }
    }
}
