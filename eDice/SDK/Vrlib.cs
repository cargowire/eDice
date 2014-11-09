using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// E-Dice wrapper
    /// </summary>
    internal class Vrlib
    {
        private static uint WM_USER = 0x0400;
        public static uint WM_EDICE = WM_USER + 201;
        public static uint WM_VRGESTURE = WM_USER + 202;

        [DllImport("Vrlib.dll", EntryPoint="vrCloseInteractionMessageHandle")]
        public static extern IntPtr CloseInteractionMessageHandle(IntPtr hInteraction);

        [DllImport("Vrlib.dll", EntryPoint="vrGetInteractionState")]
        public static extern bool GetInteractionState(IntPtr hHandle, uint StateType, IntPtr interactionData, ref uint cbSize);

        [DllImport("Vrlib.dll", EntryPoint="vrSetInteractionState")]
        public static extern bool SetInteractionState(IntPtr hHandle, uint StateType, IntPtr pInteractionData, uint cbSize);

        [DllImport("Vrlib.dll", EntryPoint="vrRegisterInteractionWindow")]
        public static extern IntPtr Register(uint type, IntPtr hWnd);

        [DllImport("Vrlib.dll", EntryPoint = "vrUnregisterInteractionWindow")]
        public static extern void Unregister(IntPtr hVr);

        [DllImport("Vrlib.dll", EntryPoint = "vrGetInteractionMessageInfo")]
        public static extern void GetInteractionMessageInfo(IntPtr hInteraction, IntPtr pInteractionData, ref uint pbSize);
    }
}