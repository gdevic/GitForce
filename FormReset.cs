using System;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormReset : Form
    {
        /// <summary>
        /// Command string that is being built based on the selected options
        /// </summary>
        public string Cmd = "";

        public FormReset()
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormResetFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Radio button to select reset type has been changed
        /// </summary>
        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if(rb.Checked)
            {
                switch (rb.Tag.ToString())
                {
                    case "0":
                        Cmd = "--soft";
                        break;
                    case "1":
                        Cmd = "--mixed";
                        break;
                    case "2":
                        Cmd = "--hard";
                        break;
                }
            }
        }
    }
}
