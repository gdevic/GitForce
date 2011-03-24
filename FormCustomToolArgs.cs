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
    /// <summary>
    /// Form to ask user for arguments before running a custom tool.
    /// </summary>
    public partial class FormCustomToolArgs : Form
    {
        /// <summary>
        /// Constructor that takes:
        ///     form title string (used to display description of the query)
        ///     initial argument string
        ///     flag whether to show or now Browse... button
        /// </summary>
        public FormCustomToolArgs(string desc, string args, bool isBrowse)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            Text = desc;
            btBrowse.Visible = isBrowse;
            comboArgs.Text = args;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormCustomToolArgsFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Return the final argument string
        /// </summary>
        public string GetArgs()
        {
            return comboArgs.Text;
        }

        /// <summary>
        /// Add a user-selected file to the arguments
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                comboArgs.Text += openFile.FileName;
            }
        }
    }
}
