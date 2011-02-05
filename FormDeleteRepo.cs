using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace git4win
{
    public partial class FormDeleteRepo : Form
    {
        /// <summary>
        /// Radio button selection mode
        /// </summary>
        private int radioSelection = 0;

        /// <summary>
        /// Root directory to delete from
        /// </summary>
        private string dir;

        public FormDeleteRepo(string root)
        {
            InitializeComponent();

            dir = textPath.Text = root;
        }

        /// <summary>
        /// Remove directories and files if selected
        /// </summary>
        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Depending on the selection, do the deletion:
                // 0: dont delete anythng
                // 1: delete only .git tree
                // 2: delete complete repo folder

                if (radioSelection == 1)
                {
                    Directory.Delete(dir + "\\.git", true);
                }

                if (radioSelection == 2)
                {
                    Directory.Delete(dir, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Any one of the 3 radio buttons are clicked. Store the selection.
        /// </summary>
        private void radioButtonClicked(object sender, EventArgs e)
        {
            radioSelection = int.Parse((sender as RadioButton).Tag.ToString());
        }
    }
}
