using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// This form is used for:
    /// 1. New commit - create a new commit bundle
    /// 2. Edit commit - edit an existing commit bundle
    /// 3. Commit - actually commit a bundle
    /// </summary>
    public partial class FormCommit : Form
    {
        /// <summary>
        /// Create a commit form or new/update form
        /// </summary>
        public FormCommit(bool forCommit, string description)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            textDescription.Font = Properties.Settings.Default.commitFont;

            if (forCommit == false)
            {
                checkAmend.Visible = false;
                btCommit.Text = "Update";
            }
            textDescription.Text = description;
            textDescription.SelectAll();
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormCommitFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// Set a list of files to appear in the files pane
        /// </summary>
        public void SetFiles(ClassCommit bundle)
        {
            listFiles.Items.AddRange(bundle.Files.ToArray());
            BtSelectAllClick(null, null);
        }

        /// <summary>
        /// Return a list of selected files from the files pane
        /// </summary>
        public List<string> GetFiles()
        {
            return listFiles.Items.Cast<object>().
                Where((t, i) => listFiles.GetItemCheckState(i) == CheckState.Checked).
                Select(t => t.ToString()).ToList();
        }

        /// <summary>
        /// Return the changelist description text
        /// </summary>
        public string GetDescription()
        {
            return textDescription.Text.Trim();
        }

        /// <summary>
        /// Return the boolean state of the amend checkmark
        /// </summary>
        public bool GetCheckAmend()
        {
            return checkAmend.Checked;
        }

        /// <summary>
        /// Select all files from the list
        /// </summary>
        private void BtSelectAllClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listFiles.Items.Count; i++)
                listFiles.SetItemChecked(i, true);
        }

        /// <summary>
        /// Unselect all files from the list
        /// </summary>
        private void BtUnselectAllClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listFiles.Items.Count; i++)
                listFiles.SetItemChecked(i, false);
        }

        /// <summary>
        /// Capture commit description text change event in order to enable "Submit" button
        /// when the text is non-empty.
        /// </summary>
        private void TextDescriptionTextChanged(object sender, EventArgs e)
        {
            btCommit.Enabled = textDescription.Text.Trim().Length > 0;
        }
    }
}
