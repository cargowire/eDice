using System;

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
        event EventHandler<DiceState> DiceRolled;

        /// <summary>
        /// The device has been shaken
        /// </summary>
        event EventHandler DiceShaken;

        /// <summary>
        /// The device has sent a power update
        /// </summary>
        event EventHandler<DiceState> DicePower;

        /// <summary>
        /// Devices have connected
        /// </summary>
        event EventHandler<DiceState> DiceConnect;

        /// <summary>
        /// Devices have disconnected
        /// </summary>
        event EventHandler<DiceState> DiceDisconnect;

        /// <summary>
        /// Start a match
        /// </summary>
        void StartMatch();

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