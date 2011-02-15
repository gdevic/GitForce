using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Security.Permissions;

namespace git4win
{
    /// <summary>
    /// Enhanced TextBox to be used for Commit text
    /// </summary>
    public sealed class TextBoxEx : TextBox
    {
        private readonly int _c1;   // Character column of the first line limit
        private readonly int _c2;   // Character column of the text limit
        private readonly int _x1;   // Pixel X offset of the column for the first limit
        private readonly int _x2;   // Pixel X offset of the column for the text limit

        public TextBoxEx()
        {
            // Get the wrap around columns for the first line and for all other lines
            _c1 = Convert.ToInt32(Properties.Settings.Default.commitW1);
            _c2 = Convert.ToInt32(Properties.Settings.Default.commitW2);

            // Get the current font and calculate the width sizes for 2 limits
            this.Font = Properties.Settings.Default.commitFont;
            Graphics gr = CreateGraphics();
            string s1 = new string(' ', _c1);
            string s2 = new string(' ', _c2);
            _x1 = TextRenderer.MeasureText(s1, Font).Width;
            _x2 = TextRenderer.MeasureText(s2, Font).Width;
            gr.Dispose();
        }

        /// <summary>
        /// Override TextBox WndProc since that is the only way to get to its OnPaint method
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                // Draw 2 lines showing the limits
                case NativeMethods.WM_PAINT:
                    Graphics gr = Graphics.FromHwnd(m.HWnd);
                    Pen pen = new Pen(Color.Red, 2);
                    gr.DrawLine(pen, new Point(_x1, 0), new Point(_x1, Font.Height));
                    gr.DrawLine(pen, new Point(_x2, Font.Height + 1), new Point(_x2, Height));
                    gr.Dispose();
                    break;
            }
        }

        /// <summary>
        /// This call makes sure that our red lines are always freshly repainted
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            Invalidate();
            base.OnKeyPress(e);
        }

        /// <summary>
        /// Paint the text in red if any of the lines exceed allowed wrap columns
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            bool over = false;
            int limit = _c1;

            // Use the limit of the first line first, then switch to the limit of the rest
            foreach (string line in Lines)
            {
                // Skip all empty lines (this will also skip empties at the start, making the
                // first non-empty line a true first line
                if( string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.TrimEnd().Length > limit)
                    over = true;
                limit = _c2;
            }
            ForeColor = over ? Color.Red : SystemColors.WindowText;

            base.OnTextChanged(e);
        }
    }
}
