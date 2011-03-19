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
    /// Implements the Log form.
    /// This form is never closed but is only shown or hidden as requested.
    /// </summary>
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();

            // Restore log window initial position and size
            if (WindowState == FormWindowState.Normal)
            {
                Location = Properties.Settings.Default.FormLogLocation;
                Size = Properties.Settings.Default.FormLogSize;
            }

            // Reuse the same font selected as fixed-pitch
            textBox.Font = Properties.Settings.Default.commitFont;

            // Debug builds start with log window open
#if DEBUG
            Print("Debug build.");
            Properties.Settings.Default.ShowLogWindow = true;
#endif
            Show(Properties.Settings.Default.ShowLogWindow);
        }

        /// <summary>
        /// Handle log window location updates
        /// </summary>
        private void FormLogSizeLocationChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.FormLogLocation = Location;
        }

        /// <summary>
        /// Handle log window size updates
        /// </summary>
        private void FormLogResizeEnd(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.FormLogSize = Size;
        }

        /// <summary>
        /// Form helper function that shows or hides a form
        /// </summary>
        public void Show(bool toShow)
        {
            if (toShow)
                Show();
            else
                Hide();
        }

        /// <summary>
        /// Adds a text string to the end of the text box.
        /// For performance reasons, only up to 120 characters of text are added in one call.
        /// </summary>
        public void Print(string text)
        {
            int len = Math.Min(text.Length, 120);
            textBox.Text += text.Substring(0, len).Trim() + Environment.NewLine;

            // Scroll to the bottom and move carret position
            textBox.SelectionStart = textBox.TextLength;
            textBox.ScrollToCaret();
        }

        #region Context menu handlers: Copy, Select All and Clear

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void ClearToolStripMenuItemClick(object sender, EventArgs e)
        {
            textBox.Clear();
        }

        #endregion
    }
}
