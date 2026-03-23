using System;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Simple input dialog for entering or editing a project name.
    /// </summary>
    public partial class FormProjectName : Form
    {
        /// <summary>
        /// The entered project name
        /// </summary>
        public string ProjectName
        {
            get { return textName.Text.Trim(); }
        }

        public FormProjectName(string title, string currentName)
        {
            InitializeComponent();
            Text = title;
            textName.Text = currentName ?? "";
            textName.SelectAll();
            UpdateOkButton();
        }

        private void TextNameTextChanged(object sender, EventArgs e)
        {
            UpdateOkButton();
        }

        private void UpdateOkButton()
        {
            btOK.Enabled = textName.Text.Trim().Length > 0;
        }

        /// <summary>
        /// Show the dialog and return the entered name, or null if cancelled.
        /// </summary>
        public static string GetName(string title, string currentName)
        {
            using (FormProjectName form = new FormProjectName(title, currentName))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    return form.ProjectName;
            }
            return null;
        }
    }
}
