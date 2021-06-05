using System;

namespace R4Utils.Messaging
{
    /// <summary>
    /// Represents the context of a <see cref="Message{TData, TEnum}" />
    /// </summary>
    public class MessageContext
    {
        /// <summary>
        /// The <see cref="Type"/> of the <see cref="Enum"/> used to create this <see cref="MessageContext"/>. 
        /// </summary>
        public Type EnumerationType { get; init; }
    }
}
