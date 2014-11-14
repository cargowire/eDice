using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using eDice.SDK;

namespace eDice
{
    /// <summary>
    /// A factory for IDiceRegistrations
    /// </summary>
    public static class eDice
    {
        /// <summary>
        /// Current registrations
        /// </summary>
        private static Dictionary<IntPtr, eDiceRegistrationWndProc> registrations = new Dictionary<IntPtr, eDiceRegistrationWndProc>();

        /// <summary>
        /// Registers the current process window for callbacks from VrLib
        /// </summary>
        /// <returns>A dice registration to allow access to the underlying VrLib SDK</returns>
        public static IDiceRegistration Register()
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
            
            // Intercept the WndProc of the window we have found so that we can middle-man dice messages
            // out through the registration
            var newWndProc = Marshal.GetFunctionPointerForDelegate(
                (User32.WndProcDelegate)ReplacedWndProcMessageReceived).ToInt32();
            var result = User32.SetWindowLong(window, (int)User32.WindowLongFlags.GWL_WNDPROC, newWndProc);

            if (!registrations.ContainsKey(window))
            {
                registrations.Add(
                    window,
                    new eDiceRegistrationWndProc(new IntPtr(result), w => registrations.Remove(w), registration));
            }

            return registrations[window];
        }

        /// <summary>
        /// Registers a specific Hwnd for callbacks from the eDice SDK.  It is expected that calling code
        /// will make the appropriate calls into IDiceRegistration.HandleMessage.
        /// </summary>
        /// <param name="hWnd">The hwnd</param>
        /// <returns>The IDiceRegistration</returns>
        public static IDiceRegistration Register(IntPtr hWnd)
        {
            var registration = Vrlib.Register((uint)INTERACTION_TYPE.VR_EDICE, hWnd);
            if (registration == IntPtr.Zero || registration.ToInt32() == -1)
            {
                throw new RegistrationException("Registration failed");
            }

            return new eDiceRegistration(registration);
        }

        /// <summary>
        /// The replacement WndProc method for registered Hwnds.  Delegates eDice messages through the relevant registration
        /// </summary>
        /// <param name="hwnd">The hwnd receiving the message</param>
        /// <param name="uMsg">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>An IntPtr</returns>
        internal static IntPtr ReplacedWndProcMessageReceived(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            var registration = registrations[hwnd];
            registration.HandleMessage((int)uMsg, wParam, lParam);
            return User32.CallWindowProc(registration.WndProc, hwnd, uMsg, wParam, lParam);
        }     
    }
}
