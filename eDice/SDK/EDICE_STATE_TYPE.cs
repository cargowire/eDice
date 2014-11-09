namespace eDice.SDK
{
    /// <summary>
    /// 
    /// </summary>
    internal enum EDICE_STATE_TYPE : uint
    {
        //e-dice message	
        EDICE_ROLLED = 1,
        EDICE_SHAKE,
        EDICE_DROP,			// not implement 
        EDICE_POWER,		// not implement 

        //DONGLE message	
        EDICE_CONNECT,
        EDICE_DISCONNECT,

        //set to dongle
        EDICE_START_PAIRING,	//start pairing; dongle should enter pairing mode (enable all e-dice)
        EDICE_SET_PAIRING,		//end and set pairing 
        EDICE_END_PAIRING,

        //query from dongle
        EDICE_QUERY_PAIRED,

        //internal message, only used by SDK
        //EDICE_PAIRED_CHANGING_INTERNAL, 
        //EDICE_REGISTER_WINDOW_INTERNAL,
    };
}