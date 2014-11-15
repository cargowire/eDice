using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eDice
{
    /// <summary>
    /// A wrapper for IDiceRegistrations that knows it is attached to a specific parent WndProc
    /// and calls it appropriately as well as running an action in relation to it on dispose.
    /// </summary>
    internal class eDiceRegistrationWndProc : IDiceRegistration
    {
        private readonly IDiceRegistration registration;
        private readonly Action<IntPtr, IntPtr> windowWndProcDisposeAction;

        /// <summary>
        /// Initializes a new instance of the eDiceRegistrationWndProc class
        /// </summary>
        /// <param name="window">The window</param>
        /// <param name="wndProc">The parent WndProc</param>
        /// <param name="windowWndProcDisposeAction">The action to call with the Window and WndProc on dispose</param>
        /// <param name="registration">The registration to wrap</param>
        public eDiceRegistrationWndProc(
            IntPtr window,
            IntPtr wndProc,
            Action<IntPtr, IntPtr> windowWndProcDisposeAction,
            IDiceRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            if (windowWndProcDisposeAction == null)
            {
                throw new ArgumentNullException("windowWndProcDisposeAction");
            }

            this.WndProc = wndProc;
            this.Window = window;
            this.windowWndProcDisposeAction = windowWndProcDisposeAction;

            this.registration = registration;
            this.registration.DiceRolled += this.RegistrationOnDiceRolled;
            this.registration.DiceShaken += this.RegistrationOnDiceShaken;
            this.registration.DicePower += this.RegistrationOnDicePower;
            this.registration.DongleConnected += this.RegistrationOnDongleConnected;
            this.registration.DongleDisconnected += this.RegistrationOnDongleDisconnected;
        }

        /// <summary>
        /// The parent hWnd this registration is associated to
        /// </summary>
        public IntPtr Window { get; private set; }

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
        public event EventHandler<DongleEventArgs> DongleConnected = delegate { };

        /// <summary>
        /// The dice disconnect event
        /// </summary>
        public event EventHandler<DongleEventArgs> DongleDisconnected = delegate { };

        /// <summary>
        /// Gets the paired devices
        /// </summary>
        public ReadOnlyCollection<int> ConnectedDongles
        {
            get
            {
                return this.registration.ConnectedDongles;
            }
        }

        /// <summary>
        /// Start a match
        /// </summary>
        public void Pair()
        {
            this.registration.Pair();
        }

        /// <summary>
        /// Ends a match
        /// </summary>
        public void Unpair()
        {
            this.registration.Unpair();
        }

        /// <summary>
        /// Gets paired devices
        /// </summary>
        /// <param name="dongleId">The dongle id</param>
        /// <returns>A list of paired dice ids</returns>
        public List<int> GetPairedDevices(int? dongleId = null)
        {
            return this.registration.GetPairedDevices(dongleId);
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
            this.windowWndProcDisposeAction(this.Window, this.WndProc);
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
        /// <param name="dongleEventArgs">The event args</param>
        private void RegistrationOnDongleConnected(object sender, DongleEventArgs dongleEventArgs)
        {
            this.DongleConnected(this, dongleEventArgs);
        }

        /// <summary>
        /// Fire the disconnect event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="dongleEventArgs">The event args</param>
        private void RegistrationOnDongleDisconnected(object sender, DongleEventArgs dongleEventArgs)
        {
            this.DongleDisconnected(this, dongleEventArgs);
        }
    }
}