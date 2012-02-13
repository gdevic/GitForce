using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Repo.Edit.Panels
{
    public partial class ControlGitconfig : UserControl, IRepoSettings
    {
        /// <summary>
        /// File containing repo git config settings
        /// </summary>
        private string _configFile;

        public ControlGitconfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        public void Init(ClassRepo repo)
        {
            _configFile = repo.Root + Path.DirectorySeparatorChar +
                            ".git" + Path.DirectorySeparatorChar +
                            "config";
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
            if (focused)
                userControlEditFile.LoadFile(_configFile);
            else
            {
                // As the control is losing its focus, if the text was changed, ask the user
                // to save the changes or not. This behavior is a special case since we need
                // to make changes to this config file atomic.
                if (userControlEditFile.Dirty)
                {
                    if (MessageBox.Show("Save changes to the configuration file?", "Edit config", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        userControlEditFile.SaveFile(_configFile);
                    else
                        userControlEditFile.LoadFile(_configFile);
                }
            }
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
            if (userControlEditFile.Dirty)
                userControlEditFile.SaveFile(_configFile);
        }
    }
}
