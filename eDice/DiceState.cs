namespace eDice
{
    /// <summary>
    /// The state of a particular dice
    /// </summary>
    public class DiceState
    {
        /// <summary>
        /// Gets or sets the dice id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the dice value
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Gets or sets the dice power value
        /// </summary>
        public int? Power { get; set; }
    }
}