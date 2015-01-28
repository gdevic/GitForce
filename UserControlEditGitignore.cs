using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
                {
                    // Read a configuration file which may use Windows or Unix specific newline code:
                    // CR = 0D  \r
                    // LF = 0A  \n
                    // Windows OS uses CR LF, Unix OS uses LF only
                    string content = sr.ReadToEnd();
                    content = content.Replace("\r", String.Empty);        // Remove 0D (\r) character
                    content = content.Replace("\n", Environment.NewLine); // Make sure we use OS-specific newline
                    textBox.Text = content;
                }
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
                using (StreamWriter sw = new StreamWriter(file))
                {
                    // Write the content of the text box while trimming each line and ignoring empty ones
                    // IMPORTANT: Write using the OS-specific newline! Required setting: core.autocrlf true
                    for (int i = 0; i < textBox.Lines.Length; i++)
                    {
                        if (textBox.Lines[i].Trim().Length > 0)
                            sw.WriteLine(textBox.Lines[i].Trim());
                    }
                }
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
