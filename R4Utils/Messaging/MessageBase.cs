using System;
using R4Utils.Messaging.Exceptions;

namespace R4Utils.Messaging
{
    #if FALSE
    /// <summary>
    /// Contains information shared for all <see cref="Message{TData}"/> types
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// The <see cref="Enum"/> containing the message context indices
        /// </summary>
        protected static Type MessageContextIndex { get; private set; } = typeof(object);

        /// <summary>
        /// The function returning the <see cref="MessageContext"/> for the index in the enum, provided some data
        /// </summary>
        protected static Func<int, Type, object?, MessageContext> BasicContextGetter { get; private set; } = (_, _, _)
            => throw new NotImplementedException(nameof(BasicContextGetter));

        /// <summary>
        /// Whether <see cref="BasicSetUp"/> has already been called without failing
        /// </summary>
        protected static bool AlreadyBasicSetUp { get; private set; } = false;

        /// <summary>
        /// Call this method to set up the message handling.
        /// MUST be called once before the first <see cref="Message{TData, TEnum}"/> can be created.
        /// MAY NOT be called thereafter.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> that identifies the <see cref="MessageContext"/>
        /// instances that may be used.</param>
        /// <param name="basicContextGetter">The function returning the <see cref="MessageContext"/> for an index
        /// in <see cref="enumType"/>, given some <see cref="object"/>.</param>
        /// <exception cref="AlreadySetUpException">Thrown when this method was already called.</exception>
        /// <exception cref="ArgumentException">Thrown when <see cref="enumType"/> is
        /// not an <see cref="Enum"/>.</exception>
        public static void BasicSetUp(Type enumType, Func<int, Type, object?, MessageContext> basicContextGetter)
        {
            if (AlreadyBasicSetUp)
                throw new AlreadySetUpException(nameof(BasicSetUp));
            
            if (enumType.IsEnum == false)
                throw new ArgumentException($"The provided ${nameof(Type)} is no ${nameof(Enum)}.",
                    nameof(enumType));

            AlreadyBasicSetUp = true;
            MessageContextIndex = enumType;
            BasicContextGetter = basicContextGetter;
        }
    }
#endif
}
