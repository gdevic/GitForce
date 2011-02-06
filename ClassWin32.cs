using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace git4win
{
    /// <summary>
    /// Class containing NativeMethods interoperbility helper functions
    /// </summary>
    class NativeMethods
    {
        public const int SB_BOTTOM = 0x0007;
        public const int WM_VSCROLL = 0x0115;
        public const int HWND_BROADCAST = 0xffff;

        public static readonly uint WmShowme = RegisterWindowMessage("WM_SHOWME");

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
    }
}
