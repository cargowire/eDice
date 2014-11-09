using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_END_PAIRING
    /// typedef struct _EDICE_END_PAIRING_INFOR{
    ///     DONGLE_ID	id;
    /// }EDICE_END_PAIRING_INFOR,*PEDICE_END_PAIRING_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class EDICE_END_PAIRING_INFOR
    {
        int id;
    }
}