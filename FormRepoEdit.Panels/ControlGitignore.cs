using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormRepoEdit_Panels
{
    public partial class ControlGitignore : UserControl, IRepoSettings
    {
        /// <summary>
        /// File containing the excludes patterns
        /// </summary>
        private string excludesFile = null;

        public ControlGitignore()
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
            excludesFile = Path.Combine(repo.root, ".git", "info", "exclude");
            userControlEditGitignore.LoadGitIgnore(excludesFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
            userControlEditGitignore.SaveGitIgnore(excludesFile);
        }
    }
}
