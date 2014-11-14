using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// E-Dice wrapper
    /// </summary>
    internal class Vrlib
    {
        /// <summary>
        /// The WM_USER message identifier
        /// </summary>
        private static uint WM_USER = 0x0400;

        /// <summary>
        /// The EDICE message identifier
        /// </summary>
        public static uint WM_EDICE = WM_USER + 201;

        /// <summary>
        /// The VRGesture message identifier
        /// </summary>
        public static uint WM_VRGESTURE = WM_USER + 202;

        /// <summary>
        /// Gets dice interaction message information
        /// </summary>
        /// <param name="hInteraction">The input handle in the LPARAM of the message</param>
        /// <param name="pInteractionData">Pointer to a buffer to receive data about the interaction</param>
        /// <param name="pbSize">The size, in bytes, of the interactionData</param>
        [DllImport("Vrlib.dll", EntryPoint = "vrGetInteractionMessageInfo")]
        public static extern void GetInteractionMessageInfo(IntPtr hInteraction, IntPtr pInteractionData, ref uint pbSize);

        /// <summary>
        /// Closes an interaction with the e-dice
        /// </summary>
        /// <param name="hInteraction">The interaction input handle in the LPARAM of the message</param>
        /// <returns>An IntPtr</returns>
        [DllImport("Vrlib.dll", EntryPoint="vrCloseInteractionMessageHandle")]
        public static extern IntPtr CloseInteractionMessageHandle(IntPtr hInteraction);

        /// <summary>
        /// Gets the interaction state
        /// </summary>
        /// <param name="hHandle">The handle returned by RegisterInteractionWindow</param>
        /// <param name="StateType">The type of interaction</param>
        /// <param name="interactionData">A pointer to a buffer to receieve data for the interaction</param>
        /// <param name="cbSize">The size, in bytes, of the interactionData</param>
        /// <returns>True on success, otherwise false</returns>
        [DllImport("Vrlib.dll", EntryPoint = "vrGetInteractionState")]
        public static extern bool GetInteractionState(IntPtr hHandle, uint StateType, IntPtr interactionData, ref uint cbSize);

        /// <summary>
        /// Sets the interaction state
        /// </summary>
        /// <param name="hHandle">The handle returned by RegisterInteractionWindow</param>
        /// <param name="StateType">The type of interaction</param>
        /// <param name="pInteractionData">A pointer to a buffer to set</param>
        /// <param name="cbSize">The size, in bytes, of the interactionData</param>
        /// <returns>True on success, otherwise false</returns>
        [DllImport("Vrlib.dll", EntryPoint="vrSetInteractionState")]
        public static extern bool SetInteractionState(IntPtr hHandle, uint StateType, IntPtr pInteractionData, uint cbSize);

        /// <summary>
        /// Registers an e-dice device to interact with
        /// </summary>
        /// <param name="type">The type of interaction we are interested in</param>
        /// <param name="hWnd">The hwnd that will receive notifications</param>
        /// <returns>A handle that the application can use to interact via VrLib with the device, else INVALID_HANDLE_VALUE</returns>
        [DllImport("Vrlib.dll", EntryPoint="vrRegisterInteractionWindow")]
        public static extern IntPtr Register(uint type, IntPtr hWnd);

        /// <summary>
        /// Unregisters an e-device to no longer interact with
        /// </summary>
        /// <param name="hVr">The handle that was returned by a call to Register</param>
        [DllImport("Vrlib.dll", EntryPoint = "vrUnregisterInteractionWindow")]
        public static extern void Unregister(IntPtr hVr);
    }
}