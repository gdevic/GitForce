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
        /// Contains the description of a previous commit, used by the amend option.
        /// </summary>
        private readonly string amendText;

        /// <summary>
        /// Create a commit form or new/update form
        /// </summary>
        public FormCommit(bool forCommit, string description)
        {
            InitializeComponent();
            ClassWinGeometry.Restore(this);

            textDescription.Font = Properties.Settings.Default.commitFont;

            // If we are updating the change, the function is slightly different:
            // The default text is different and the checkbox copies a previous
            // description for the amend operation.
            if (forCommit == false)
            {
                btCommit.Text = "Update";
                checkAmend.Text = "Copy description of a previous change for the amend operation";
            }

            // Fetch the description of a previous commit for the amend option
            ExecResult result = App.Repos.Current.Run("log --pretty=format:%s%n%b -1");
            if(result.Success())
            {
                amendText = result.stdout;
                // BUG: We are losing newlines with App.Repos.Current.Run. At least insert one after the subject line.
                if (amendText.IndexOf(Environment.NewLine) > 0)
                    amendText = amendText.Insert(amendText.IndexOf(Environment.NewLine), Environment.NewLine);
            }
            else
                amendText = "Unknown";
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
        /// On form load, set the focus to the text box.
        /// This is a way to have it start with all text selected.
        /// </summary>
        private void FormCommitLoad(object sender, EventArgs e)
        {
            textDescription.Focus();
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
        /// Checkbox 'amend' has been checked/unchecked
        /// </summary>
        private void CheckAmendCheckedChanged(object sender, EventArgs e)
        {
            if (checkAmend.Checked)
            {
                // If the text in the description box has not been changed,
                // swap it with the text of a previous commit (for amend)
                if (GetDescription() == "Default" || GetDescription() == "Update")
                    textDescription.Text = amendText;

                // In any case copy the text to the OS clipboard
                Clipboard.SetText(amendText);
            }
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
                for (int y = 1; y < lines.Length; y++ )
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
