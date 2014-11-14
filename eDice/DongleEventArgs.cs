using System;
using System.Collections.Generic;

namespace eDice
{
    /// <summary>
    /// Represents an event with dongle information attached
    /// </summary>
    public class DongleEventArgs : EventArgs
    {
        /// <summary>
        /// Initialize a new instance of the DongleEventArgs class
        /// </summary>
        public DongleEventArgs(List<int> dongleIds)
        {
            this.DongleIds = dongleIds;
        }

        /// <summary>
        /// The dongle ids
        /// </summary>
        public List<int> DongleIds { get; private set; }
    }
}
