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
    internal class EDICE_DISCONNECT_INFOR : DiceConnectionInformation
    {
    }
}