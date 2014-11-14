using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// typedef struct _DICE_STATE
    /// {
    ///    DICE_ID id;    // hardware ID of dice
    ///    INT32  value;  // depend on State Types
    ///    INT32  power;
    /// }DICE_STATE,*PDICE_STATE;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class DICE_STATE
    {
        /// <summary>
        /// The hardware id of the device
        /// </summary>
        public Int32 id;

        /// <summary>
        /// A value related to the identified dice.
        /// </summary>
        public Int32 value;

        /// <summary>
        /// The power of the device.  Zero indicates low power.
        /// </summary>
        public Int32 power;
    }
}