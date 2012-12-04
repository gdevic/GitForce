using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Repo.Edit.Panels
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
        public void Init(ClassRepo repo)
        {
            textBoxUserName.Text = ClassConfig.GetLocal(repo, "user.name");
            textBoxUserEmail.Text = ClassConfig.GetLocal(repo, "user.email");

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
        public void ApplyChanges(ClassRepo repo)
        {
            if (textBoxUserName.Tag != null)
            {
                // Change the repo config and our internal variable so we dont need to reload
                ClassConfig.SetLocal(repo, "user.name", textBoxUserName.Text);
                repo.UserName = textBoxUserName.Text;
                textBoxUserName.Tag = null;
            }

            if (textBoxUserEmail.Tag != null)
            {
                // Change the repo config and our internal variable so we dont need to reload
                ClassConfig.SetLocal(repo, "user.email", textBoxUserEmail.Text);
                repo.UserEmail = textBoxUserEmail.Text;
                textBoxUserEmail.Tag = null;
            }
        }
    }
}
