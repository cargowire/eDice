using System;
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
        /// <summary>
        /// The message header
        /// </summary>
        public MESSAGE_HEADER header;
        
        /// <summary>
        /// The first byte of the data (header will contain size information to allow marshalling of the data)
        /// </summary>
        public IntPtr data;
    }
}