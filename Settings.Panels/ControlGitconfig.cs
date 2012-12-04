using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlGitconfig : UserControl, IUserSettings
    {
        /// <summary>
        /// File containing user gitconfig settings
        /// </summary>
        private string configFile;

        public ControlGitconfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            configFile = Path.Combine(App.UserHome, ".gitconfig");
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
            if (focused)
                userControlEditFile.LoadFile(configFile);
            else
            {
                // As the control is losing its focus, if the text was changed, ask the user
                // to save the changes or not. This behavior is a special case since we need
                // to make changes to this config file atomic.
                if (userControlEditFile.Dirty)
                {
                    if (MessageBox.Show("Save changes to the configuration file?", "Edit .gitconfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        userControlEditFile.SaveFile(configFile);
                    else
                        userControlEditFile.LoadFile(configFile);
                }
            }
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            if (userControlEditFile.Dirty)
                userControlEditFile.SaveFile(configFile);
        }
    }
}
