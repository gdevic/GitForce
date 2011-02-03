using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win.FormOptions_Panels
{
    public partial class ControlDiff :  UserControl, IUserSettings
    {
        public ControlDiff()
        {
            InitializeComponent();
        }

        private List<ClassDiff.TDiff> diffs = null;
        private int checkedIndex = -1;

        /// <summary>
        /// Initialize pertinent settings
        /// </summary>
        /// <param name="options">All git global settings</param>
        public void Init(string[] options)
        {
            // Add names of diff tools that were found on the system
            diffs = App.Diff.diffs;
            string activeName = Properties.Settings.Default.DiffActiveName;
            foreach(var s in diffs)
                listBoxDiffs.Items.Add(s.name, s.name==activeName);
        }

        /// <summary>
        /// Apply changed settings
        /// </summary>
        public void ApplyChanges()
        {
            Properties.Settings.Default.DiffActiveName = diffs[checkedIndex].name;
            ClassDiff.Configure(diffs, diffs[checkedIndex]);
        }

        /// <summary>
        /// User clicked on an diff tool to select it as the active one
        /// </summary>
        private void listBoxDiffs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                foreach (int i in listBoxDiffs.CheckedIndices)
                    listBoxDiffs.SetItemCheckState(i, CheckState.Unchecked);
                checkedIndex = e.Index;
            }
        }
    }
}
