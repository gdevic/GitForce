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
    public partial class ControlDiff : UserControl, IUserSettings
    {
        public ControlDiff()
        {
            InitializeComponent();
        }

        private List<ClassDiff.Diff> _diffs;
        private int _checkedIndex = -1;

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Add names of diff tools that were found on the system
            _diffs = App.Diff.Diffs;
            string activeName = Properties.Settings.Default.DiffAppHelper;
            foreach (var s in _diffs)
                listBoxDiffs.Items.Add(s.Name, s.Name == activeName);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.DiffAppHelper = _diffs[_checkedIndex].Name;
            ClassDiff.Configure(_diffs, _diffs[_checkedIndex]);
        }

        /// <summary>
        /// User clicked on an diff tool to select it as the active one
        /// </summary>
        private void ListBoxDiffsItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                foreach (int i in listBoxDiffs.CheckedIndices)
                    listBoxDiffs.SetItemCheckState(i, CheckState.Unchecked);
                _checkedIndex = e.Index;
            }
        }
    }
}
