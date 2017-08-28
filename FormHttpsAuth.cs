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
    }
}
