using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// This is a fancy text box which is used by the direct command line box.
    /// 
    /// The callback it implements, TextReady(), will trigger when the user
    /// presses Enter in the box. It will be called only when there is a non-empty
    /// text in the box (user does not have to check for empty text.)
    /// Also, text will be trimmed at the front and back from extra spaces.
    /// 
    /// The text box will clear itself after the TextReady() has been sent.
    /// </summary>
    public partial class TextBoxEx : TextBox
    {
        public delegate void TextReadyEventHandler(object sender, string text);

        // Declare an event to trigger on Enter key
        [Category("Action")]
        [Description("Occurs when the text is ready for consumption")]
        public event TextReadyEventHandler TextReady;

        // List of strings entered so far to hold the history of input
        // Each string is unique
        private readonly List<string> history = new List<string>();
        // Index into history when browsing it (Up / Down)
        private int iH = 0;

        public TextBoxEx() {}

        // Handler called on every key press into the subclassed TextBox
        // Using this handler we capture cursor up and down keys
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Up suggests a history entry
                case Keys.Up:
                    if (iH > 0)
                    {
                        iH--;
                        Text = history[iH];
                        Select(Text.Length, 0);
                    }
                    e.Handled = true;
                    break;

                // Down suggests a history entry
                case Keys.Down:
                    if (iH < history.Count-1)
                    {
                        iH++;
                        Text = history[iH];
                        Select(Text.Length, 0);
                    }
                    e.Handled = true;
                    break;                    
            }
            base.OnKeyDown(e);
        }

        // Handler called on every key press into the subclassed TextBox
        // Using this handler we capture ASCII keys
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                // On Enter, process the buffer and send a method
                case (char)Keys.Enter:
                    // Trim the line from the extra spaces at both ends
                    Text = Text.Trim();

                    // Send a message that the text is ready (if there is any text available)
                    if (TextReady != null && Text.Length > 0)
                        TextReady(this, Text);

                    if (Text.Length > 0 && !history.Contains(Text))
                        history.Add(Text);

                    iH = history.Count();
                    Text = string.Empty;
                    
                    e.Handled = true;
                    break;

                // ESC clears the text box entry
                case (char)Keys.Escape:
                    Text = string.Empty;
                    e.Handled = true;       // This also avoids the chime
                    break;
            }
            base.OnKeyPress(e);
        }
    }
}
