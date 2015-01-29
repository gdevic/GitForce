using System;
using System.Diagnostics;
using System.IO;
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
            ClassWinGeometry.Restore(this);

            // Add our main print function callback delegate
            App.PrintLogMessage += Print;

            // WAR: On Linux, remove status bar resizing grip (since it does not work under X)
            if (ClassUtils.IsMono())
                statusStrip.SizingGrip = false;

            if (App.AppLog != null)
                Print("Logging: " + App.AppLog, MessageType.General);

            // Prints only in Debug build...
            Debug("Debug build.");
        }

        /// <summary>
        /// Form is closing.
        /// </summary>
        private void FormLogFormClosing(object sender, FormClosingEventArgs e)
        {
            ClassWinGeometry.Save(this);

            // Remove our print function delegate
            App.PrintLogMessage -= Print;
        }

        /// <summary>
        /// Form helper function that shows or hides a form
        /// </summary>
        public void ShowWindow(bool toShow)
        {
            if( Visible==false && toShow)
                Show();

            if(Visible && toShow==false)
                Hide();
        }

        /// <summary>
        /// Adds a text string to the end of the text box.
        /// For performance reasons, only up to 120 characters of text are added in one call.
        /// This is a thread-safe call.
        /// </summary>
        private void Print(string text, MessageType type)
        {
            if (textBox.InvokeRequired)
                textBox.BeginInvoke((MethodInvoker)(() => Print(text, type)));
            else
            {
                try
                {
                    // Mirror the text to the file log, if enabled
                    if (App.AppLog != null)
                        using (StreamWriter sw = File.AppendText(App.AppLog))
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "|" + text);

                    // Print the text into the textbox
                    int len = Math.Min(text.Length, 120);
                    textBox.Text += text.Substring(0, len).Trim() + Environment.NewLine;

                    // Scroll to the bottom and move carret position
                    textBox.SelectionStart = textBox.TextLength;
                    textBox.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Prints a message only in debug build
        /// This is a thread-safe call.
        /// </summary>
        [Conditional("DEBUG")]
        private void Debug(string text)
        {
            Print(text, MessageType.Debug);
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