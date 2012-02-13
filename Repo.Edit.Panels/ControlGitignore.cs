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
        public void Init(ClassRepo repo)
        {
            _excludesFile = repo.Root + Path.DirectorySeparatorChar +
                            ".git" + Path.DirectorySeparatorChar +
                            "info" + Path.DirectorySeparatorChar +
                            "exclude";
            userControlEditGitignore.LoadGitIgnore(_excludesFile);
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
            userControlEditGitignore.SaveGitIgnore(_excludesFile);
        }
    }
}
