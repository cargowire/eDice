using System;

namespace eDice
{
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

        event EventHandler<DiceState> DicePower;

        event EventHandler<DiceState> DiceConnect;

        event EventHandler<DiceState> DiceDisconnect;

        void StartMatch();

        bool HandleMessage(int message, IntPtr wParam, IntPtr lParam);
    }
}