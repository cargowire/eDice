using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// Represents dice connection information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal abstract class DiceConnectionInformation
    {
        /// <summary>
        /// The length of the id array
        /// </summary>
        public uint num;

        /// <remarks>
        /// This will not be the pointer, this will just be the next byte in memory after 'num'
        /// We need to manually figure out what comes here based on the SDK definition (lists dongle
        /// of ids).  This variable provides a hook for Marshal offsets etc.
        /// </remarks>
        public IntPtr id;
    }
}
