using System;
using System.Text;
using R4Utils.Messaging.Exceptions;

namespace R4Utils.Messaging
{
    /// <summary>
    /// Represents the context of a <see cref="Message{TData, TEnum}" />
    /// </summary>
    public class MessageContext<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// The entry of the this <see cref="MessageContext{TEnum}"/> in the <see cref="Enum"/>
        /// used to create this instance. 
        /// </summary>
        public TEnum ContextIdentifier { get; init; }

        /// <summary>
        /// The message that can get displayed to the user.
        /// </summary>
        public string MessageText { get; init; }

        /// <summary>
        /// The severity assigned to this instance.
        /// The interpretation of this value is not fixed.
        /// </summary>
        public int Severity { get; init; }

        /// <summary>
        /// The file containing the caller creating
        /// the <see cref="Message{TData,TEnum}"/> for which this instance has been created.
        /// </summary>
        public string SourceCompilationUnit { get; init; }
        /// <summary>
        /// The method creating
        /// the <see cref="Message{TData,TEnum}"/> for which this instance has been created.
        /// </summary>
        public string SourceMethod { get; init; }
        /// <summary>
        /// The line in the <see cref="SourceCompilationUnit"/> in which
        /// the <see cref="Message{TData,TEnum}"/> for which this instance has been created,
        /// was created.
        /// </summary>
        public int SourceLine { get; init; }

        public static MessageContext<TEnum> Create(TEnum enumEntry, string messageText, int severity,
            string sourceCompilationUnit, string sourceMethod, int sourceLine)
            => new(enumEntry, messageText, severity, sourceCompilationUnit, sourceMethod, sourceLine);

        protected MessageContext(TEnum contextIdentifier, string messageText, int severity, string sourceCompilationUnit,
            string sourceMethod, int sourceLine)
        {
            ContextIdentifier = contextIdentifier;
            MessageText = messageText;
            Severity = severity;
            SourceCompilationUnit = sourceCompilationUnit;
            SourceMethod = sourceMethod;
            SourceLine = sourceLine;
        }

        /// <summary>
        /// Get all data from this instance as a <see cref="string"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> in the form 'Message: ...\n Identifier: ...\n Severity:
        /// ...\n Message occurred at ... in line ... in ...'.</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Message: \"{MessageText}\"");
            sb.Append(Environment.NewLine);
            sb.Append($"Identifier: {ContextIdentifier}");
            sb.Append(Environment.NewLine);
            sb.Append($"Severity: {Severity}");
            sb.Append(Environment.NewLine);
            sb.Append($"Message occurred at '{SourceCompilationUnit} in line {SourceLine} in {SourceMethod}");
            return sb.ToString();
        }

        /// <summary>
        /// Get a <see cref="MessageException"/> from this instance.
        /// </summary>
        /// <returns>The <see cref="MessageException"/> containing the information about this instance.</returns>
        public MessageException AsException() => new(ToString());
    }
}
