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
        /// <summary>
        /// The dongle id
        /// </summary>
        public int id;

        /// <summary>
        /// The number of state structures to expect in states
        /// </summary>
        public uint num;

        /// <summary>
        /// An array of dice states
        /// </summary>
        public IntPtr states;
    }
}