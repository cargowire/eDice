using System.Runtime.InteropServices;

namespace eDice.SDK
{
    /// <summary>
    /// EDICE_SET_PAIRING & EDICE_QUERY_PAIRED & EDICE_PAIRED_CHANGING
    /// typedef struct _EDICE_PAIRING_INFOR{
    ///     DONGLE_ID	id;
    ///     UINT32		num;
    ///     DICE_ID		dices[0];
    /// }EDICE_PAIRING_INFOR,*PEDICE_PAIRING_INFOR;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class EDICE_PAIRING_INFOR
    {
        int id;
        int num;
        //TODO: int dices[0];
    }
}