using System.Windows.Forms;

namespace GitForce
{
    public partial class FormRemoteEdit : Form
    {
        public FormRemoteEdit(ClassRepo repo)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            userControlRemotesEdit.SetRepo(repo);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormRemoteEditFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }
    }
}
