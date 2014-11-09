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
        public Int32 id;
        public Int32 value;
        public Int32 power;
    }
}