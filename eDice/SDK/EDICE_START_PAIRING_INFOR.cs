using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_START_PAIRING
    /// typedef struct _EDICE_START_PAIRING_INFOR{
    ///     DONGLE_ID	id;
    /// }EDICE_START_PAIRING_INFOR,*PEDICE_START_PAIRING_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EDICE_START_PAIRING_INFOR
    {
        /// <summary>
        /// The dongle id
        /// </summary>
        public int id;
    }
}