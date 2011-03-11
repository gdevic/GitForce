using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win
{
    public partial class FormDiffMissing : Form
    {
        public FormDiffMissing()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User clicked on a link to a diff utility.
        /// Open it in the web browser.
        /// </summary>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string link = (sender as LinkLabel).Tag.ToString();
            Process.Start(link);
        }
    }
}
