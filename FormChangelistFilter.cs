using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormChangelistFilter : Form
    {
        /// <summary>
        /// Final git log filter string
        /// </summary>
        public string gitFilter = "";

        public FormChangelistFilter()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            dateTimeBefore.Value = DateTime.Today;
            dateTimeAfter.Value = DateTime.Today;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormChangelistFilterFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// User clicked on the OK button, format the git filter string
        /// </summary>
        private void BtOkClick(object sender, EventArgs e)
        {
            gitFilter = "";

            if (checkBoxMessage.Checked)
                gitFilter += " --grep=\"" + textBoxMessage.Text + "\"";

            if (checkBoxAuthor.Checked)
                gitFilter += " --author=\"" + textBoxAuthor.Text + "\"";

            if (checkBoxBefore.Checked)
            {
                DateTime dt = dateTimeBefore.Value;
                gitFilter += " --before=" + String.Format("{0:yyyy/MM/dd}", dt);
            }

            if (checkBoxAfter.Checked)
            {
                DateTime dt = dateTimeAfter.Value;
                gitFilter += " --after=" + String.Format("{0:yyyy/MM/dd}", dt);                
            }

            App.PrintLogMessage("Condition: " + gitFilter);
        }
    }
}