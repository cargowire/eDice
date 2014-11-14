using System;

namespace eDice
{
    /// <summary>
    /// The state of a particular dice
    /// </summary>
    public class DiceState : EventArgs
    {
        public int Value { get; set; }
    }
}