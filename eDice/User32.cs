using System;
using System.Runtime.InteropServices;

namespace eDice
{
    /// <summary>
    /// The pinvoke mappings for User32 methods
    /// </summary>
    public static class User32
    {
        /// <summary>
        /// Window long flags (for use with setting long window values such as the WndProc address)
        /// </summary>
        public enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }

        /// <summary>
        /// The struct that is passed to a HookProc when using GWL_WNDPROC long flag
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr lparam;
            public IntPtr wparam;
            public int message;
            public IntPtr hwnd;
        }

        /// <summary>
        /// Represents a WndProc handling function
        /// </summary>
        /// <param name="hWnd">The window</param>
        /// <param name="msg">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>An IntPtr</returns>
        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// A delegate for enumerating available windows
        /// </summary>
        /// <param name="hwnd">The hWnd found</param>
        /// <param name="lParam"></param>
        /// <returns>True to continue enumerating, false if not</returns>
        public delegate bool EnumWindowsProc(int hwnd, int lParam);

        /// <summary>
        /// A delegate for hook procedures (that act as 'listeners' on other windows wndprocs
        /// </summary>
        /// <param name="code">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>An IntPtr</returns>
        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Get the foreground hWnd handle
        /// </summary>
        /// <returns>The hWnd handle</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Request to enumerate all available windows
        /// </summary>
        /// <param name="lpEnumFunc">The callback function to execute</param>
        /// <param name="lParam">The lParam</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// Gets the specified window thread process id
        /// </summary>
        /// <param name="hWnd">The window</param>
        /// <param name="lpdwProcessId">The process id</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// Set a windows procedure hook
        /// </summary>
        /// <param name="code">The message identifier</param>
        /// <param name="func">The function</param>
        /// <param name="hInstance">The handle to the window to hook</param>
        /// <param name="threadID">The thread id</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

        /// <summary>
        /// Unregister a windows procedure hook
        /// </summary>
        /// <param name="hhk">The hook</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// Call the next wnd proc hook (from within an existing hook)
        /// </summary>
        /// <param name="hhk">The hook</param>
        /// <param name="nCode">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Send a message to the windows message queue
        /// </summary>
        /// <param name="hWnd">The window</param>
        /// <param name="Msg">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Set a long window value
        /// </summary>
        /// <param name="hWnd">The window</param>
        /// <param name="nIndex">The index</param>
        /// <param name="dwNewLong">The new value</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// Call a window procedure directly
        /// </summary>
        /// <param name="lpPrevWndFunc">The wnd proc</param>
        /// <param name="hWnd">The hWnd</param>
        /// <param name="Msg">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
