using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.ExtensionMethods
{
    public static class Win32
    {
        [DllImport("User32.dll", EntryPoint = "LockWindowUpdate", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LockWindow(IntPtr Handle);

        [DllImport("User32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("User32.dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
