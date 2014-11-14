using System;

namespace eDice
{
    /// <summary>
    /// There was a problem registering the eDice with the hWnd
    /// </summary>
    public class RegistrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the RegistrationException class
        /// </summary>
        /// <param name="message">The message</param>
        public RegistrationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RegistrationException class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        public RegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}