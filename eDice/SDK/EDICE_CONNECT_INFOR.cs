using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_CONNECT
    /// typedef struct _EDICE_CONNECT_INFOR{
    ///     UINT32		num;
    ///     DONGLE_ID	id[0];
    /// }EDICE_CONNECT_INFOR,*PEDICE_CONNECT_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class EDICE_CONNECT_INFOR
    {
        public int num;

        public IntPtr id;
    }
}