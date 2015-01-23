using System;
using System.IO;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormDeleteRepo : Form
    {
        /// <summary>
        /// Radio button selection mode
        /// </summary>
        private int radioSelection;

        /// <summary>
        /// Root directory to delete from
        /// </summary>
        private readonly string dir;

        public FormDeleteRepo(string root)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            dir = textPath.Text = root;
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
            // Move CWD away as it prevents Windows OS to cleanly delete directory
            Directory.SetCurrentDirectory(App.AppHome);

            bool ret = true;
            // Depending on the selection, do the deletion:
            // 0: dont delete anythng
            // 1: delete only working files
            // 2: delete only .git tree
            // 3: delete complete repo folder

            if (radioSelection == 1)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                ret = ClassUtils.DeleteFolder(dirInfo, true, true);     // Preserve .git, preserve root folder
            }

            if (radioSelection == 2)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir + Path.DirectorySeparatorChar + ".git");
                ret = ClassUtils.DeleteFolder(dirInfo, false, false);    // Remove .git, remove root folder (.git)
            }

            if(radioSelection == 3)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                ret = ClassUtils.DeleteFolder(dirInfo, false, false);   // Remove .git, remove root folder
            }

            if (ret == false)
                MessageBox.Show("Some files could not be removed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Any one of the 3 radio buttons are clicked. Store the selection.
        /// </summary>
        private void RadioButtonClicked(object sender, EventArgs e)
        {
            radioSelection = int.Parse((sender as RadioButton).Tag.ToString());
        }
    }
}
