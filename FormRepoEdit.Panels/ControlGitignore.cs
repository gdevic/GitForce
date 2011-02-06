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
        private string _excludesFile;

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
            _excludesFile = Path.Combine(repo.Root, ".git", "info", "exclude");
            userControlEditGitignore.LoadGitIgnore(_excludesFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
            userControlEditGitignore.SaveGitIgnore(_excludesFile);
        }
    }
}
