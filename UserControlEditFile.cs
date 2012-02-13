using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    public partial class UserControlEditFile : UserControl
    {
        /// <summary>
        /// True if the content of a text box is modified.
        /// </summary>
        public bool Dirty;

        public UserControlEditFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads specified text file to edit
        /// </summary>
        public bool LoadFile(string file)
        {
            bool result = true;
            labelFileName.Text = file;
            textBox.Text = "";
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        textBox.Text += sr.ReadLine() + Environment.NewLine;
                    }
                }
            }
            catch (Exception)
            {
                textBox.Text = "(Unable to load file)";
                result = false;
            }
            Dirty = false;
            textBox.Enabled = result;
            return result;
        }

        /// <summary>
        /// Saves the edited content of the text box into a file
        /// </summary>
        public bool SaveFile(string file)
        {
            // Dont attempt to save non-loaded content
            if (textBox.Enabled == false)
                return false;

            bool result = true;
            try
            {
                using (StreamWriter sw = new StreamWriter(file))
                    sw.WriteLine(textBox.Text);
                labelFileName.Text = file;
                Dirty = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// User modified the content of a text box. Mark it as dirty.
        /// </summary>
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }
    }
}
