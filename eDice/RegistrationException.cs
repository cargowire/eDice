using System;

namespace eDice
{
    /// <summary>
    /// There was a problem registering the eDice with the hWnd
    /// </summary>
    public class RegistrationException : Exception
    {
        public RegistrationException(string message)
            : base(message)
        {
        }

        public RegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}