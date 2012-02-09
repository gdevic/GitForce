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
        private string _configFile;

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
            _configFile = Path.Combine(App.UserHome, ".gitconfig");
            userControlEditFile.LoadFile(_configFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            userControlEditFile.SaveFile(_configFile);
        }
    }
}
