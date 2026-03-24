using System.Windows.Forms;

namespace GitForce.Repo.Edit.Panels
{
    public partial class ControlLocal : UserControl, IRepoSettings
    {
        public ControlLocal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Keeps the local settings of line endings
        /// </summary>
        private string crlf;

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        public void Init(ClassRepo repo)
        {
            // Initialize the line endings radio buttons
            crlf = ClassConfig.GetLocal(repo, "core.autocrlf");
            if (crlf == "true") radio1.Checked = true;
            if (crlf == "input") radio2.Checked = true;
            if (crlf == "false") radio3.Checked = true;
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
            string newCrlf = string.Empty;              // Default
            if (radio1.Checked) newCrlf = "true";
            if (radio2.Checked) newCrlf = "input";
            if (radio3.Checked) newCrlf = "false";
            if (radio4.Checked) newCrlf = string.Empty; // This value will remove the setting

            if (newCrlf != crlf)
                ClassConfig.SetLocal(repo, "core.autocrlf", newCrlf);
        }
    }
}
