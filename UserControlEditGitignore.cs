using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    public partial class UserControlEditGitignore : UserControl
    {
        /// <summary>
        /// Current file name, displayed on the control
        /// </summary>
        private string FileName
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
        /// Loads specified text file as a gitignore file
        /// </summary>
        public bool LoadGitIgnore(string file)
        {
            bool result = true;
            try
            {
                using (StreamReader sr = new StreamReader(file))
                    textBox.Text = sr.ReadToEnd();
                FileName = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit gitignore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Saves the edited content of a gitignore listbox into a file
        /// </summary>
        public bool SaveGitIgnore(string file)
        {
            bool result = true;
            try
            {
                using(StreamWriter sw = new StreamWriter(file))
                    sw.WriteLine(textBox.Text);
                FileName = file;
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
        private void BtAddClick(object sender, EventArgs e)
        {
            List<string> items = (from string s in listFilters.SelectedItems select s.Split(' ').First()).ToList();
            items.InsertRange(0, textBox.Lines);
            textBox.Lines = items.Distinct().ToArray();
        }
    }
}
