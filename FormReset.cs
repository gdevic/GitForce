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
    public partial class FormReset : Form
    {
        /// <summary>
        /// Command string that is being built based on the selected options
        /// </summary>
        public string cmd = "";

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
                        cmd = "--soft";
                        break;
                    case "1":
                        cmd = "--mixed";
                        break;
                    case "2":
                        cmd = "--hard";
                        break;
                }
            }
        }
    }
}
