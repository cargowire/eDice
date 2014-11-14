using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eDice
{
    /// <summary>
    /// Represents an event with dice states attached
    /// </summary>
    public class DiceStateEventArgs : EventArgs
    {
        /// <summary>
        /// Initialize a new instance of the DiceStateEventArgs class
        /// </summary>
        public DiceStateEventArgs(List<DiceState> diceStates)
        {
            this.DiceStates = diceStates;
        }

        /// <summary>
        /// The dice states
        /// </summary>
        public List<DiceState> DiceStates { get; private set; }
    }
}
