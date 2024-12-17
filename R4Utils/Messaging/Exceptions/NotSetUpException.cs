using System;

namespace R4Utils.Messaging.Exceptions
{
    /// <summary>
    /// Thrown when using a method that requires set up that was not performed.
    /// </summary>
    public class NotSetUpException : Exception
    {
        public NotSetUpException(string name) : base($"Calling ${name} requires setting it up first.")
        {
        }
    }
}
