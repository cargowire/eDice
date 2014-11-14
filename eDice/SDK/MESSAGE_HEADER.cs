using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// typedef struct _MESSAGE_HEADER
    /// {
    ///     UINT32	size; // size of all data; total length of VR_MESSAGE_INFORMATION
    ///     INT32	type; // represent State Types: one of EDICE_STATE_TYPE
    /// 	
    /// } MESSAGE_HEADER, *PMESSAGE_HEADER;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MESSAGE_HEADER
    {
        /// <summary>
        /// Size of all data; total length of VR_MESSAGE_INFORMATION
        /// </summary>
        public uint size;

        /// <summary>
        /// Represent State Types: one of EDICE_STATE_TYPE
        /// </summary>
        public int type;
    }
}