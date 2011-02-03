using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class UserControlEditGitignore : UserControl
    {
        /// <summary>
        /// Current file name, displayed on the control
        /// </summary>
        private string fileName
        {
            set
            {
                labelFileName.Text = value;
            }
        }

        public UserControlEditGitignore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads specified text file as gitignore file
        /// </summary>
        public bool LoadGitIgnore(string file)
        {
            bool result = true;
            try
            {
                textBox.Text = File.ReadAllText(file).Replace("\n", Environment.NewLine);
                fileName = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit gitignore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Saves the edited content of gitignore into a file
        /// </summary>
        public bool SaveGitIgnore(string file)
        {
            bool result = true;
            try
            {
                File.WriteAllText(file, textBox.Text.ToString());
                fileName = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit gitignore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Add all selected items to the gitignore text box
        /// Dont add duplicates.
        /// </summary>
        private void btAdd_Click(object sender, EventArgs e)
        {
            List<string> items = new List<string>();
            foreach (string s in listFilters.SelectedItems)
                items.Add(s.Split(' ').First());
            items.InsertRange(0, textBox.Lines);
            textBox.Lines = items.Distinct().ToArray();
        }
    }
}
