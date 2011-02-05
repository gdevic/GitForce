using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace git4win
{
    /// <summary>
    /// Class containing Win32 interoperbility helper functions
    /// </summary>
    internal class Win32
    {
        public const int SB_BOTTOM = 0x0007;
        public const int WM_VSCROLL = 0x0115;
        public const int HWND_BROADCAST = 0xffff;

        public static readonly int WmShowme = RegisterWindowMessage("WM_SHOWME");

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
    }
}
