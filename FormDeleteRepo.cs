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

        /// <summary>
        /// Recursive deletion function signals that some files could not be removed
        /// </summary>
        private bool _errorDeleting;

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
            try
            {
                // Depending on the selection, do the deletion:
                // 0: dont delete anythng
                // 1: delete only working files
                // 2: delete only .git tree
                // 3: delete complete repo folder
                _errorDeleting = false;

                if (_radioSelection == 1)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(_dir);
                    DeleteRecursiveFolder(dirInfo, true, true);     // Preserve .git, preserve root folder
                }

                if (_radioSelection == 2)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(_dir + Path.DirectorySeparatorChar + ".git");
                    DeleteRecursiveFolder(dirInfo, false, false);    // Remove .git, remove root folder (.git)
                }

                if(_radioSelection == 3)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(_dir);
                    DeleteRecursiveFolder(dirInfo, false, false);   // Remove .git, remove root folder
                }

                if( _errorDeleting )
                    throw new ClassException("Some files could not be removed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete a directory and all files and subdirectories under it.
        /// TODO: This particular case could probably be optimized: do we really need 2 booleans coming in
        /// </summary>
        private void DeleteRecursiveFolder(DirectoryInfo dirInfo, bool fPreserveGit, bool fPreserveRootFolder)
        {
            foreach (var subDir in dirInfo.GetDirectories())
            {
                if( fPreserveGit==false || !subDir.Name.EndsWith(".git"))
                    DeleteRecursiveFolder(subDir, false, false);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
                try
                {
                    file.Delete();
                }
                catch { _errorDeleting = true; }
            }

            if(fPreserveRootFolder==false)
            {
                try
                {
                    dirInfo.Delete();
                }
                catch { _errorDeleting = true; }
            }
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
