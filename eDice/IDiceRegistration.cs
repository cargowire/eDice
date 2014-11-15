using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eDice
{
    /// <summary>
    /// Represents a registered eDice SDK
    /// </summary>
    public interface IDiceRegistration : IDisposable
    {
        /// <summary>
        /// The dice has been rolled
        /// </summary>
        event EventHandler<DiceStateEventArgs> DiceRolled;

        /// <summary>
        /// The device has been shaken
        /// </summary>
        event EventHandler<DiceStateEventArgs> DiceShaken;

        /// <summary>
        /// The dice has been dropped
        /// </summary>
        event EventHandler<DiceStateEventArgs> DiceDropped;
        
        /// <summary>
        /// The device has sent a power update
        /// </summary>
        event EventHandler<DiceStateEventArgs> DicePower;

        /// <summary>
        /// Devices have connected
        /// </summary>
        event EventHandler<DongleEventArgs> DongleConnected;

        /// <summary>
        /// Devices have disconnected
        /// </summary>
        event EventHandler<DongleEventArgs> DongleDisconnected;

        /// <summary>
        /// Gets the paired devices
        /// </summary>
        ReadOnlyCollection<int> PairedDevices { get; } 

        /// <summary>
        /// Start a match
        /// </summary>
        void Pair();

        /// <summary>
        /// Ends a match
        /// </summary>
        void Unpair();

        /// <summary>
        /// Handle a WndProc message
        /// </summary>
        /// <param name="message">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>True if the message was handled, otherwise false</returns>
        bool HandleMessage(int message, IntPtr wParam, IntPtr lParam);
    }
}