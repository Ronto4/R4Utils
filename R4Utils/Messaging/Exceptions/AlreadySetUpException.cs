using System;

namespace R4Utils.Messaging.Exceptions
{
    /// <summary>
    /// Thrown when a set up method has been called more than once.
    /// </summary>
    public class AlreadySetUpException : Exception
    {
        public AlreadySetUpException(string methodName)
            : base($"The method ${methodName} may not be called more than once.")
        {
        }
    }
}
