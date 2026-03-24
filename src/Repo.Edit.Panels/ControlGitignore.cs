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
            // Use git rev-parse to get the actual git directory (handles submodules correctly)
            ExecResult result = repo.Run("rev-parse --git-dir");
            string gitDir = result.Success()
                ? result.stdout.Trim()
                : Path.Combine(repo.Path, ".git");

            // If the path is relative, make it absolute
            if (!Path.IsPathRooted(gitDir))
                gitDir = Path.Combine(repo.Path, gitDir);

            excludesFile = Path.Combine(gitDir, "info", "exclude");
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
