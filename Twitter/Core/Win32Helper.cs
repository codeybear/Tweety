using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Tweety.Core
{
    static class NativeMethods
    {
        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_NONTOPMOST = -2;
        private const int HWND_TOPMOST = -1;
        private const int HWND_TOP = 0;

        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(
             int hWnd,                  // window handle
             int hWndInsertAfter,       // placement-order handle
             int x,                     // horizontal position
             int y,                     // vertical position
             int cx,                    // width
             int cy,                    // height
             uint uFlags);              // window positioning flags

        /// <summary>
        /// Make a window topmost, without making it permanently topmost
        /// </summary>
        /// <param name="window"></param>
        public static void ShowWindowTopMost(System.Windows.Window window) {
            IntPtr windowHandle = new WindowInteropHelper(window).Handle;

            SetWindowPos(windowHandle.ToInt32(),
                         HWND_TOPMOST,
                         Convert.ToInt32(window.Left),
                         Convert.ToInt32(window.Top),
                         Convert.ToInt32(window.Width),
                         Convert.ToInt32(window.Height),
                         SWP_SHOWWINDOW);

            SetWindowPos(windowHandle.ToInt32(),
                         HWND_NONTOPMOST,
                         Convert.ToInt32(window.Left),
                         Convert.ToInt32(window.Top),
                         Convert.ToInt32(window.Width),
                         Convert.ToInt32(window.Height),
                         SWP_SHOWWINDOW);
        }
    }
}
