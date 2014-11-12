using System;

namespace eDice
{
    public class eDiceRegistrationWndProc : IDiceRegistration
    {
        private readonly IDiceRegistration registration;

        private readonly Action<IntPtr> wndProcDisposeAction;

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

        public IntPtr WndProc { get; private set; }

        public event EventHandler<DiceState> DiceRolled = delegate { };

        public event EventHandler DiceShaken = delegate { };

        public event EventHandler<DiceState> DicePower = delegate { };

        public event EventHandler<DiceState> DiceConnect = delegate { };

        public event EventHandler<DiceState> DiceDisconnect = delegate { };

        public void StartMatch()
        {
            this.registration.StartMatch();
        }

        public bool HandleMessage(int message, IntPtr wParam, IntPtr lParam)
        {
            return this.registration.HandleMessage(message, wParam, lParam);
        }

        public void Dispose()
        {
            this.wndProcDisposeAction(this.WndProc);
            this.registration.Dispose();
        }

        private void RegistrationOnDiceShaken(object sender, EventArgs eventArgs)
        {
            this.DiceShaken(this, eventArgs);
        }

        private void RegistrationOnDiceRolled(object sender, DiceState diceState)
        {
            this.DiceRolled(this, diceState);
        }

        private void RegistrationOnDicePower(object sender, DiceState diceState)
        {
            this.DicePower(this, diceState);
        }

        private void RegistrationOnDiceConnect(object sender, DiceState diceState)
        {
            this.DiceConnect(this, diceState);
        }

        private void RegistrationOnDiceDisconnect(object sender, DiceState diceState)
        {
            this.DiceDisconnect(this, diceState);
        }
    }
}