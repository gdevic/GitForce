using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win.Settings.Panels
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
                Path.Combine(App.AppHome, ".gitexcludesfile")
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
            ClassConfig.SetGlobal("core.excludesfile", _excludesFile);
        }
    }
}
