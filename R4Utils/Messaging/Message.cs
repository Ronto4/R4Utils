using System;
using System.Runtime.CompilerServices;
using R4Utils.Messaging.Exceptions;

namespace R4Utils.Messaging
{
    /// <summary>
    /// Represents a message sent between methods
    /// </summary>
    /// <typeparam name="TData">The type of the value being returned</typeparam>
    /// <typeparam name="TEnum">The type of the <see cref="Enum"/> that is
    /// used to infer the <see cref="MessageContext"/></typeparam>
    public class Message<TData, TEnum>/* : MessageBase*/ where TEnum : Enum
    {
        /// <summary>
        /// The actual value being returned
        /// </summary>
        public TData Data { get; init; }

        /// <summary>
        /// The message context containing more information about this instance
        /// </summary>
        public MessageContext<TEnum> MessageContext { get; init; }

        /// <summary>
        /// The function returning the <see cref="string"/> message and severity for an element
        /// of <see cref="TEnum"/>, given some <see cref="TData"/>.
        /// </summary>
        // protected static Func<TEnum, TData, MessageContext<TEnum>> ContextGetter { get; set; } = (enumEntry, data)
        //     => BasicContextGetter(Convert.ToInt32(enumEntry), typeof(TEnum), data);
        protected static Func<TEnum, TData, (string, int)> ContextGetter { get; set; } = (_, _) =>
            throw new NotImplementedException();

        /// <summary>
        /// Whether <see cref="SetUp"/> has already been called without failing
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        protected static bool AlreadySetUp { get; private set; } = false;

        /// <summary>
        /// Call this method to set up the message handling for
        /// this specific combination of <see cref="TData"/> and <see cref="TEnum"/>.
        /// MUST be called once before the first <see cref="Message{TData, TEnum}"/> can be created.
        /// MAY NOT be called thereafter.
        /// </summary>
        /// <param name="contextGetter">The function returning
        /// the <see cref="string"/> message and severity for an element
        /// of <see cref="TEnum"/>, given some <see cref="TData"/>.</param>
        /// <exception cref="AlreadySetUpException">Thrown when this method was already called.</exception>
        public static void SetUp(Func<TEnum, TData, (string, int)> contextGetter)
        {
            if (AlreadySetUp)
                throw new AlreadySetUpException(nameof(SetUp));
            
            ContextGetter = contextGetter;
            AlreadySetUp = true;
        }

        public static Message<TData, TEnum> Create(TData data, TEnum enumEntry,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (AlreadySetUp == false/* && AlreadyBasicSetUp == false*/)
                throw new NotSetUpException(nameof(Create));

            if (data is null)
                throw new ArgumentNullException(nameof(data),
                    $"A ${nameof(Message<TData, TEnum>)} may not be created with null ${nameof(data)}.");

            (string message, int severity) = ContextGetter(enumEntry, data);
            MessageContext<TEnum> context = MessageContext<TEnum>.Create(enumEntry, message, severity,
                sourceFilePath, memberName, sourceLineNumber);
            return new(data, context);
        }
        
        protected Message(TData data, MessageContext<TEnum> messageContext)
        {
            Data = data;
            MessageContext = messageContext;
        }
    }
}
