using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
{
    public partial class FormRemoteEdit : Form
    {
        public FormRemoteEdit(ClassRepo repo)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            userControlRemoteEdit.SetRepo(repo);
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
