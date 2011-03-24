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

            // If the file does not exist, reset it
            if (_excludesFile == null)
                _excludesFile = Path.Combine(App.AppHome, ".gitexcludesfile");

            _excludesFile = _excludesFile.Split('\n').Last();

            if (!File.Exists(_excludesFile))
                try
                {
                    File.CreateText(_excludesFile).Close();
                }
                catch (Exception ex)
                {
                    App.PrintStatusMessage("Error loading " + _excludesFile + ": " + ex.Message);
                }

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
