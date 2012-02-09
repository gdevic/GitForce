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
            userControlEditFile.LoadFile(_configFile);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges(ClassRepo repo)
        {
            userControlEditFile.SaveFile(_configFile);
        }
    }
}
