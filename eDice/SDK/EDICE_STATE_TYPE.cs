namespace eDice.SDK
{
    /// <summary>
    /// E-Dice state types
    /// </summary>
    internal enum EDICE_STATE_TYPE : uint
    {
        //// E-dice messages
        EDICE_ROLLED = 1,
        EDICE_SHAKE,

        /// <summary>
        /// Not implemented
        /// </summary>
        EDICE_DROP,

        /// <summary>
        /// Not implemented
        /// </summary>
        EDICE_POWER,

        //// DONGLE messages
        EDICE_CONNECT,
        EDICE_DISCONNECT,

        //// Set to dongle

        /// <summary>
        /// Start pairing; dongle should enter pairing mode (enable all e-dice)
        /// </summary>
        EDICE_START_PAIRING,

        /// <summary>
        /// End and set pairing 
        /// </summary>
        EDICE_SET_PAIRING,
        EDICE_END_PAIRING,

        // Query from dongle
        EDICE_QUERY_PAIRED,

        // Internal messages -  only used by SDK
        // EDICE_PAIRED_CHANGING_INTERNAL, 
        // EDICE_REGISTER_WINDOW_INTERNAL,
    }
}