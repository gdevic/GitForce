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
        private string excludesFile = null;

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
            excludesFile = options.FirstOrDefault(s => s.Contains("core.excludesfile"));
            if (excludesFile == null)
                excludesFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gitexcludesfile");
            else
                excludesFile = excludesFile.Split('\n').Last();

            // If the user file does not exists, create it
            if (!File.Exists(excludesFile))
                File.CreateText(excludesFile).Close();

            userControlEditGitignore.LoadGitIgnore(excludesFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            userControlEditGitignore.SaveGitIgnore(excludesFile);
            ClassConfig.Set("core.excludesfile", excludesFile);
        }
    }
}
