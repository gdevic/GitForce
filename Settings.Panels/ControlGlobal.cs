using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlGlobal : UserControl, IUserSettings
    {
        public ControlGlobal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Keeps the global settings of line endings
        /// </summary>
        private string crlf;

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Initialize the line endings radio buttons
            crlf = ClassConfig.GetGlobal("core.autocrlf");
            if (crlf == "true")  radio1.Checked = true;
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
        public void ApplyChanges()
        {
            // Default core.autocrlf setting is "true" on Windows and "false" on Linux
            string newCrlf = ClassUtils.IsMono() ? "false" : "true";
            if (radio1.Checked) newCrlf = "true";
            if (radio2.Checked) newCrlf = "input";
            if (radio3.Checked) newCrlf = "false";

            if (newCrlf != crlf)
                ClassConfig.SetGlobal("core.autocrlf", newCrlf);
        }
    }
}
