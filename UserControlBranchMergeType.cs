using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class UserControlBranchMergeStyle : UserControl
    {
        /// <summary>
        /// Defines help text to print when a merge option is selected
        /// </summary>
        private static Dictionary<string, string> help = new Dictionary<string, string>()
        {
            { "fastforward", "Help for fastforward" },
            { "commit", "Help for commit" },
            { "resolve", "Help for resolve" },
            { "recursive", "Help for recursive" },
            { "octopus", "Help for octopus" },
            { "ours", "Help for ours" },
            { "subtree", "Help for subtree" }
        };

        /// <summary>
        /// Currently selected merge style
        /// </summary>
        private string current = null;

        public UserControlBranchMergeStyle()
        {
            InitializeComponent();

            comboStrategy.Text = "resolve";
        }

        /// <summary>
        /// Set the style to the given name
        /// </summary>
        public void SetStyle(string style)
        {
            rb0.Checked = rb1.Checked = rb2.Checked = false;

            switch (style)
            {
                case "fastforward" :
                    rb0.Checked = true;
                    break;
                case "commit":
                    rb1.Checked = true;
                    break;
                case "resolve":
                    rb2.Checked = true;
                    comboStrategy.SelectedIndex = 0;
                    break;
                case "recursive":
                    rb2.Checked = true;
                    comboStrategy.SelectedIndex = 1;
                    break;
                case "octopus":
                    rb2.Checked = true;
                    comboStrategy.SelectedIndex = 2;
                    break;
                case "ours":
                    rb2.Checked = true;
                    comboStrategy.SelectedIndex = 3;
                    break;
                case "subtree":
                    rb2.Checked = true;
                    comboStrategy.SelectedIndex = 4;
                    break;
            }
        }

        /// <summary>
        /// Return currently selected style name
        /// </summary>
        public string GetStyle()
        {
            if (rb2.Checked == true)
                return comboStrategy.Text;
            return current;
        }

        /// <summary>
        /// Called on radio button style changed
        /// </summary>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == true)
            {
                current = (sender as RadioButton).Tag.ToString();
                if (current == "strategy")
                    current = comboStrategy.Text;
                labelHelp.Text = help[current];
            }
        }

        /// <summary>
        /// Called on combo style selection changed
        /// </summary>
        private void comboStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelHelp.Text = help[(sender as ComboBox).Text];
        }
    }
}
