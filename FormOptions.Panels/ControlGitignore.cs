using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormOptions_Panels
{
    public partial class ControlGitignore : UserControl, IUserSettings
    {
        /// <summary>
        /// File containing user gitignore settings
        /// </summary>
        private string _excludesFile;

        public ControlGitignore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Get a path to user excludes file, or create that file if it is not defined
            _excludesFile = options.FirstOrDefault(s => s.Contains("core.excludesfile"));
            _excludesFile = _excludesFile == null ? 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gitexcludesfile")
                : _excludesFile.Split('\n').Last();

            // If the user file does not exists, create it
            if (!File.Exists(_excludesFile))
                File.CreateText(_excludesFile).Close();

            userControlEditGitignore.LoadGitIgnore(_excludesFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            userControlEditGitignore.SaveGitIgnore(_excludesFile);
            ClassConfig.Set("core.excludesfile", _excludesFile);
        }
    }
}
