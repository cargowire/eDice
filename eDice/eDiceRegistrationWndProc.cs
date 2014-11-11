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
        }

        public IntPtr WndProc { get; private set; }

        public event EventHandler<DiceState> DiceRolled = delegate { };

        public event EventHandler DiceShaken = delegate { };

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
    }
}