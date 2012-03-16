using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GitForce
{
    /// <summary>
    /// Class containing NativeMethods interoperbility helper functions
    /// </summary>
    static class NativeMethods
    {
        public const int SB_BOTTOM = 0x0007;
        public const int WM_CLOSE = 0x0010;
        public const int WM_PAINT = 0x000F;
        public const int WM_VSCROLL = 0x0115;
        public const int HWND_BROADCAST = 0xffff;
        public const int WM_COMMAND = 0x0111;

        public static readonly uint WmShowme = RegisterWindowMessage("WM_SHOWME");

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Attaches a console so we can use Console class to print.
        /// This is needed only on Windows implementation where WinForms app detaches from its console.
        /// </summary>
        public static void AttachConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);            
        }
    }
}
