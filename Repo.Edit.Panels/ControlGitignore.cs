using System.IO;
using System.Windows.Forms;

namespace GitForce.Repo.Edit.Panels
{
    public partial class ControlGitignore : UserControl, IRepoSettings
    {
        /// <summary>
        /// File containing the excludes patterns
        /// </summary>
        private string excludesFile;

        public ControlGitignore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        public void Init(ClassRepo repo)
        {
            excludesFile = repo.Root + Path.DirectorySeparatorChar +
                            ".git" + Path.DirectorySeparatorChar +
                            "info" + Path.DirectorySeparatorChar +
                            "exclude";
            userControlEditGitignore.LoadGitIgnore(excludesFile);
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
            userControlEditGitignore.SaveGitIgnore(excludesFile);
        }
    }
}
