using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormDeleteRepo : Form
    {
        /// <summary>
        /// Radio button selection mode
        /// </summary>
        private int _radioSelection;

        /// <summary>
        /// Root directory to delete from
        /// </summary>
        private readonly string _dir;

        public FormDeleteRepo(string root)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            _dir = textPath.Text = root;
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormDeleteRepoFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Remove directories and files if selected
        /// </summary>
        private void BtDeleteClick(object sender, EventArgs e)
        {
            bool ret = true;
            // Depending on the selection, do the deletion:
            // 0: dont delete anythng
            // 1: delete only working files
            // 2: delete only .git tree
            // 3: delete complete repo folder

            if (_radioSelection == 1)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_dir);
                ret = ClassUtils.DeleteFolder(dirInfo, true, true);     // Preserve .git, preserve root folder
            }

            if (_radioSelection == 2)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_dir + Path.DirectorySeparatorChar + ".git");
                ret = ClassUtils.DeleteFolder(dirInfo, false, false);    // Remove .git, remove root folder (.git)
            }

            if(_radioSelection == 3)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_dir);
                ret = ClassUtils.DeleteFolder(dirInfo, false, false);   // Remove .git, remove root folder
            }

            if (ret == false)
                MessageBox.Show("Some files could not be removed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Any one of the 3 radio buttons are clicked. Store the selection.
        /// </summary>
        private void RadioButtonClicked(object sender, EventArgs e)
        {
            _radioSelection = int.Parse((sender as RadioButton).Tag.ToString());
        }
    }
}
