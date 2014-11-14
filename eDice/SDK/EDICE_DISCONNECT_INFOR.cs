using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_DISCONNECT
    /// typedef struct _EDICE_DISCONNECT_INFOR{
    ///     UINT32		num;
    ///     DONGLE_ID	id[0];
    /// }EDICE_DISCONNECT_INFOR,*PEDICE_DISCONNECT_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class EDICE_DISCONNECT_INFOR
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