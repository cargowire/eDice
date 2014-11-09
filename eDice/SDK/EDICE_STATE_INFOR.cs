using System;
using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_ROLLED,
    /// EDICE_SHAKE,
    /// EDICE_DROP,
    /// EDICE_POWER,
    /// typedef struct _EDICE_STATE_INFOR{
    ///    DONGLE_ID	id;
    ///    UINT32		num;	//
    ///    DICE_STATE	states[0];
    /// }EDICE_STATE_INFOR,*PEDICE_STATE_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EDICE_STATE_INFOR
    {
        public int id;
        public uint num;
        public IntPtr states;
    }
}