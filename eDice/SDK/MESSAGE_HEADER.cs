using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// typedef struct _MESSAGE_HEADER
    /// {
    ///     UINT32	size;			// size of all data; total length of VR_MESSAGE_INFORMATION
    ///     INT32	type;			// represent State Types: one of EDICE_STATE_TYPE
    /// 	
    /// } MESSAGE_HEADER, *PMESSAGE_HEADER;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MESSAGE_HEADER
    {
        uint size;
        int type;
    }
}