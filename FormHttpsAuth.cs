using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormHttpsAuth : Form
    {
        /// <summary>
        /// Combination of user name and password in a single string
        /// </summary>
        public string PassCombo
        {
            get { return textUsername.Text.Trim() + "\t" + textPassword.Text.Trim(); }
            set
            {
                textUsername.Text = "";
                textPassword.Text = "";
                string[] combo = value.Trim().Split('\t');
                if (combo.Length == 1)
                    textPassword.Text = combo[0];
                else if (combo.Length == 2)
                {
                    textUsername.Text = combo[0];
                    textPassword.Text = combo[1];
                }
            }
        }

        /// <summary>
        /// Return user name and password fields as typed in the control
        /// </summary>
        public string Username => textUsername.Text.Trim();
        public string Password => textPassword.Text.Trim();

        /// <summary>
        /// Form constructor
        /// </summary>
        public FormHttpsAuth()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormHttpsAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Show or hide password field
        /// </summary>
        private void CheckShowCheckedChanged(object sender, System.EventArgs e)
        {
            textPassword.UseSystemPasswordChar = !checkShow.Checked;
        }

        /// <summary>
        /// Validator for the user name field: must start with a letter and ...
        /// </summary>
        private void TextUsernameTextChanged(object sender, System.EventArgs e)
        {
            string s = textUsername.Text.Trim();

            Regex r = new Regex("^[a-zA-Z][a-zA-Z0-9_]*$");
            btOK.Enabled = r.IsMatch(s);
        }

        // This is a really lame validator for the password field
        // We simply want to avoid some 'dangerous' characters, including spaces
        private void TextPasswordTextChanged(object sender, System.EventArgs e)
        {
            string s = textPassword.Text.Trim();
            btOK.Enabled = (s.Length > 0) && s.IndexOfAny(@" \/".ToCharArray()) == -1;
        }
    }
}
