using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GitForce.Settings.Panels
{
    public partial class ControlDoubleClick : UserControl, IUserSettings
    {
        // Selected radio button option number
        private string option = Properties.Settings.Default.DoubleClick;

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

            if (!rb.ContainsKey(option))
                option = "1";
            rb[option].Checked = true;
            comboApps.Enabled = option == "2";
        }

        /// <summary>
        /// Control received a focus (true) or lost a focus (false)
        /// </summary>
        public void Focus(bool focused)
        {
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.DoubleClickProgram = comboApps.Text;
            Properties.Settings.Default.DoubleClick = option;
        }

        /// <summary>
        /// Radio button clicked
        /// </summary>
        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                option = rb.Tag.ToString();
                comboApps.Enabled = rb.Tag.ToString() == "2";
            }
        }
    }
}
