using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win.Settings.Panels
{
    public partial class ControlDoubleClick : UserControl, IUserSettings
    {
        // Selected radio button option number
        private string _option = Properties.Settings.Default.DoubleClick;

        public ControlDoubleClick()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Load a list of programs from the application settings
            string[] progs = Properties.Settings.Default.EditViewPrograms.Split(("\0").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in progs)
                comboApps.Items.Add(s);
            comboApps.Text = Properties.Settings.Default.DoubleClickProgram;

            // Check the radio button with the currently stored action
            Dictionary<string, RadioButton> rb = new Dictionary<string, RadioButton> {
                { "0", radioButton0 }, { "1", radioButton1 }, { "2", radioButton2 } };

            rb[_option].Checked = true;
            comboApps.Enabled = _option == "2";
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.DoubleClickProgram = comboApps.Text;
            Properties.Settings.Default.DoubleClick = _option;
        }

        /// <summary>
        /// Radio button clicked
        /// </summary>
        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                _option = rb.Tag.ToString();
                comboApps.Enabled = rb.Tag.ToString() == "2";
            }
        }
    }
}
