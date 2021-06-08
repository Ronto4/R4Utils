using System;

namespace R4Utils.Messaging.Exceptions
{
    /// <summary>
    /// This <see cref="Exception"/> may be created from
    /// a <see cref="Message{TData,TEnum}"/> or a <see cref="MessageContext{TEnum}"/>.
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        /// Creates an instance from just the <see cref="MessageContext{TEnum}"/> without any data.
        /// </summary>
        /// <param name="messageInformation">A <see cref="string"/> containing information
        /// about the <see cref="MessageContext{TEnum}"/> used to create this instance.</param>
        public MessageException(string messageInformation)
            : base($"The following message was thrown: {messageInformation}")
        {
        }

        /// <summary>
        /// Creates an instance from a <see cref="MessageContext{TEnum}"/> and some <see cref="data"/>.
        /// </summary>
        /// <param name="messageInformation">A <see cref="string"/> containing information
        /// about the <see cref="MessageContext{TEnum}"/> used to create this instance.</param>
        /// <param name="data">The data enclosed in the <see cref="Message{TData,TEnum}"/> used
        /// to create this instance.</param>
        public MessageException(string messageInformation, object data) : base(messageInformation)
        {
            MessageData = data;
        }

        /// <summary>
        /// The data that was enclosed in the <see cref="Message{TData,TEnum}"/> used to create this instance.
        /// Is null when just a <see cref="MessageContext{TEnum}"/> was used to create this instance.
        /// </summary>
        public object? MessageData { get; init; } = null;
    }
}
