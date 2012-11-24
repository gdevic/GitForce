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
    /// 1. Commit - actually commit a full merge operation
    /// 2. Edit commit - edit an existing merge commit
    /// </summary>
    public partial class FormCommitMerge : Form
    {
        public FormCommitMerge(bool forCommit, string description)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            textDescription.Font = Properties.Settings.Default.commitFont;

            // If we are updating the change, the function is slightly different.
            if (forCommit == false)
                btCommit.Text = "Update";

            textDescription.Text = description;
            textDescription.SelectAll();
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void FormCommitMergeFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);
        }

        /// <summary>
        /// On form load, set the focus to the text box.
        /// This is a way to have it start with all text selected.
        /// </summary>
        private void FormCommitMergeLoad(object sender, EventArgs e)
        {
            textDescription.Focus();
        }

        /// <summary>
        /// Set a list of files to appear in the files pane
        /// </summary>
        public void SetFiles(ClassCommit bundle)
        {
            listFiles.Items.AddRange(bundle.Files.ToArray());
        }

        /// <summary>
        /// Return the changelist description text
        /// </summary>
        public string GetDescription()
        {
            return textDescription.Text.Trim();
        }

        /// <summary>
        /// Capture commit description text change event in order to enable "Submit" button
        /// when the text is non-empty.
        /// The secondary purpose is to check the text wrapping margins.
        /// </summary>
        private void TextDescriptionTextChanged(object sender, EventArgs e)
        {
            btCommit.Enabled = textDescription.Text.Trim().Length > 0;

            string[] lines = textDescription.Text.Trim().Split((Environment.NewLine).ToCharArray()).ToArray();
            int w1 = lines[0].Length;
            int w2 = 0;

            // Check the rest of the lines (find the maximum)
            if (lines.Length > 1)
                for (int y = 1; y < lines.Length; y++)
                    if (lines[y].Length > w2)
                        w2 = lines[y].Length;

            labelWidth.Text = String.Format("Text span: First line {0}/{1}, body {2}/{3}",
                w1, Properties.Settings.Default.commitW1,
                w2, Properties.Settings.Default.commitW2);

            // Print the cursor location
            labelCursor.Text = string.Format("({0},{1})",
                textDescription.SelectionStart - textDescription.GetFirstCharIndexOfCurrentLine(),
                textDescription.GetLineFromCharIndex(textDescription.SelectionStart));

            // Color the text and the label in red if the span was reached))
            if (w1 > Properties.Settings.Default.commitW1 || w2 > Properties.Settings.Default.commitW2)
                textDescription.ForeColor = labelWidth.ForeColor = Color.Red;
            else
                textDescription.ForeColor = labelWidth.ForeColor = SystemColors.ControlText;
        }
    }
}
