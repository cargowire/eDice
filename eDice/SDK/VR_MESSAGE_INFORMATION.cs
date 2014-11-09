using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDice uniform format:  EDICE_HEADER | Message
    /// Message format:
    /// typedef struct _VR_MESSAGE_INFORMATION{
    ///     MESSAGE_HEADER header;
    ///     char data[0];	//XXX_INFOR
    /// }VR_MESSAGE_INFORMATION,*PVR_MESSAGE_INFORMATION;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct VR_MESSAGE_INFORMATION
    {
        public MESSAGE_HEADER header;
        public EDICE_STATE_INFOR data;
    }
}