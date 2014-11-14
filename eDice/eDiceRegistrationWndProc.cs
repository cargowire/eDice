using System;

namespace eDice
{
    /// <summary>
    /// A wrapper for IDiceRegistrations that knows it is attached to a specific parent WndProc
    /// and calls it appropriately as well as running an action in relation to it on dispose.
    /// </summary>
    public class eDiceRegistrationWndProc : IDiceRegistration
    {
        private readonly IDiceRegistration registration;
        private readonly Action<IntPtr> wndProcDisposeAction;

        /// <summary>
        /// Initializes a new instance of the eDiceRegistrationWndProc class
        /// </summary>
        /// <param name="wndProc">The parent WndProc</param>
        /// <param name="wndProcDisposeAction">The action to call with the WndProc on dispose</param>
        /// <param name="registration">The registration to wrap</param>
        public eDiceRegistrationWndProc(
            IntPtr wndProc,
            Action<IntPtr> wndProcDisposeAction,
            IDiceRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            if (wndProcDisposeAction == null)
            {
                throw new ArgumentNullException("wndProcDisposeAction");
            }

            this.WndProc = wndProc;
            this.wndProcDisposeAction = wndProcDisposeAction;

            this.registration = registration;
            this.registration.DiceRolled += this.RegistrationOnDiceRolled;
            this.registration.DiceShaken += this.RegistrationOnDiceShaken;
            this.registration.DicePower += this.RegistrationOnDicePower;
            this.registration.DiceConnect += this.RegistrationOnDiceConnect;
            this.registration.DiceDisconnect += this.RegistrationOnDiceDisconnect;
        }

        /// <summary>
        /// The parent WndProc this registration is associated to
        /// </summary>
        public IntPtr WndProc { get; private set; }

        /// <summary>
        /// The dice rolled event
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceRolled = delegate { };

        /// <summary>
        /// The dice shaken event
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceShaken = delegate { };

        /// <summary>
        /// The dice power event
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DicePower = delegate { };

        /// <summary>
        /// The dice has been dropped
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceDropped = delegate { };

        /// <summary>
        /// The dice connect event
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceConnect = delegate { };

        /// <summary>
        /// The dice disconnect event
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceDisconnect = delegate { };

        /// <summary>
        /// Start a match
        /// </summary>
        public void StartMatch()
        {
            this.registration.StartMatch();
        }

        /// <summary>
        /// Handle a wnd proc message
        /// </summary>
        /// <param name="message">The message identifier</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>True if the message was handled, false if not.</returns>
        public bool HandleMessage(int message, IntPtr wParam, IntPtr lParam)
        {
            return this.registration.HandleMessage(message, wParam, lParam);
        }

        /// <summary>
        /// Dispose of this registration
        /// </summary>
        public void Dispose()
        {
            this.wndProcDisposeAction(this.WndProc);
            this.registration.Dispose();
        }

        /// <summary>
        /// Fire the shaken event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="diceState">The args</param>
        private void RegistrationOnDiceShaken(object sender, DiceStateEventArgs diceState)
        {
            this.DiceShaken(this, diceState);
        }

        /// <summary>
        /// Fire the rolled event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="diceState">The event args</param>
        private void RegistrationOnDiceRolled(object sender, DiceStateEventArgs diceState)
        {
            this.DiceRolled(this, diceState);
        }

        /// <summary>
        /// Fire the power event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="diceState">The event args</param>
        private void RegistrationOnDicePower(object sender, DiceStateEventArgs diceState)
        {
            this.DicePower(this, diceState);
        }

        /// <summary>
        /// Fire the connect event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="diceState">The event args</param>
        private void RegistrationOnDiceConnect(object sender, DiceStateEventArgs diceState)
        {
            this.DiceConnect(this, diceState);
        }

        /// <summary>
        /// Fire the disconnect event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="diceState">The event args</param>
        private void RegistrationOnDiceDisconnect(object sender, DiceStateEventArgs diceState)
        {
            this.DiceDisconnect(this, diceState);
        }
    }
}