using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git4Win.Settings.Panels
{
    public partial class ControlDiff : UserControl, IUserSettings
    {
        /// <summary>
        /// List of helper programs
        /// </summary>
        private List<AppHelper> _helpers;

        public ControlDiff()
        {
            InitializeComponent();

            textDesc.Text = "A diff utility needs to support these arguments:" + Environment.NewLine +
                            "%1 - Local file ($LOCAL)" + Environment.NewLine +
                            "%2 - Remote file ($REMOTE)";
        }

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Detect all diff utilities on the system and populate a listbox
            _helpers = ClassDiff.GetDetected();
            comboBoxPath.Items.Clear();
            foreach (var appHelper in _helpers)
                comboBoxPath.Items.Add(appHelper.Path);

            // Get our program default diff tool and set the listbox text
            AppHelper app = new AppHelper(Properties.Settings.Default.DiffAppHelper);
            comboBoxPath.Text = app.Path;
            textArgs.Text = app.Args;

            // Add the dirty (modified) value changed helper
            comboBoxPath.TextChanged += ControlDirtyHelper.ControlDirty;
            textArgs.TextChanged += ControlDirtyHelper.ControlDirty;
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            if (comboBoxPath.Tag != null || textArgs.Tag !=null)
            {
                string name = Path.GetFileNameWithoutExtension(comboBoxPath.Text);
                AppHelper app = new AppHelper(name, comboBoxPath.Text, textArgs.Text);
                Properties.Settings.Default.DiffAppHelper = app.ToString();

                ClassDiff.Configure(app);                
            }
        }

        /// <summary>
        /// Browse for a utility using a file open dialog
        /// </summary>
        private void BtBrowseClick(object sender, EventArgs e)
        {
            if(openApp.ShowDialog()==DialogResult.OK)
            {
                comboBoxPath.Text = openApp.FileName;
                textArgs.Text = "%1 %2";
            }
        }

        /// <summary>
        /// Item was selected from the drop-down list
        /// </summary>
        private void ComboBoxPathSelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox) sender;
            textArgs.Text = _helpers[senderComboBox.SelectedIndex].Args;
        }
    }
}
