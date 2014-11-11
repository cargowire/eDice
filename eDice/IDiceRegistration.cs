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

        bool HandleMessage(int message, IntPtr wParam, IntPtr lParam);
    }
}