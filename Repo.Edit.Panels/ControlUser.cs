using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win.Repo.Edit.Panels
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
            textBoxUserName.Text = ClassConfig.GetLocal(repo, "user.name");
            textBoxUserEmail.Text = ClassConfig.GetLocal(repo, "user.email");

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
                ClassConfig.SetLocal(repo, "user.name", textBoxUserName.Text);
                textBoxUserName.Tag = null;
            }

            if (textBoxUserEmail.Tag != null)
            {
                ClassConfig.SetLocal(repo, "user.email", textBoxUserEmail.Text);
                textBoxUserEmail.Tag = null;
            }
        }
    }
}
