using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using eDice.SDK;

namespace eDice
{
    public class eDice
    {
        public static eDiceRegistration Register()
        {
            // var window = GetForegroundWindow(); 
            // GetForegroundWindow does not appear to give the correct result so we enumerate
            // all windows to find it
            IntPtr window = IntPtr.Zero;
            User32.EnumWindows(
                (int hWnd, int lParam) =>
                {
                    uint processId = 0;
                    User32.GetWindowThreadProcessId(new IntPtr(hWnd), out processId);

                    if (processId == Process.GetCurrentProcess().Id)
                    {
                        window = new IntPtr(hWnd);
                        return false; // Stop enumerating
                    }

                    return true;
                },
                IntPtr.Zero);

            if (window == IntPtr.Zero)
            {
                throw new RegistrationException("Registration failed");
            }

            var registration = Register(window);
            
            // Application.AddMessageFilter(new eDiceMessageFilter(registration));
            // Unfortunately MessageFilters require a reference to System.Windows.Forms which we cannot have

            /*var hook = User32.SetWindowsHookEx(HookType.WH_CALLWNDPROC,
               HookMessage,
               IntPtr.Zero,
               checked((int)Kernel32.GetCurrentThreadId()));
            User32.UnhookWindowsHookEx(hook);*/
            // Unfortunately WindowsHook's don't appear to pick up on the values sent via Vrlib.dll

            int newWndProc = Marshal.GetFunctionPointerForDelegate(
                (User32.WndProcDelegate)registration.ReplacedWndProcMessageReceived).ToInt32();
            var result = User32.SetWindowLong(window, (int)User32.WindowLongFlags.GWL_WNDPROC, newWndProc);
            registration.oldWindowProc = (IntPtr)result;

            return registration;
        }

        public static eDiceRegistration Register(IntPtr hWnd)
        {
            var registration = Vrlib.Register((uint)INTERACTION_TYPE.VR_EDICE, hWnd);
            if (registration == IntPtr.Zero || registration.ToInt32() == -1)
            {
                throw new RegistrationException("Registration failed");
            }

            return new eDiceRegistration(registration);
        }
    }
}
