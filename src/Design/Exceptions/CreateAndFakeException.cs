using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for errors occurring within the <c>CreateAndFake</c> framework.
/// </summary>
[Serializable]
[KnownType(typeof(Exception))]
[KnownType(typeof(Exception[]))]
[KnownType(typeof(string[]))]
public abstract class CreateAndFakeException : Exception
{
    /// <inheritdoc cref="CreateAndFakeException"/>
    /// <remarks>Serialization constructor.</remarks>
    protected CreateAndFakeException()
        : base() { }

    /// <inheritdoc cref="CreateAndFakeException"/>
    /// <inheritdoc/>
    protected CreateAndFakeException(string? message)
        : base(message) { }

    /// <inheritdoc cref="CreateAndFakeException"/>
    /// <inheritdoc/>
    protected CreateAndFakeException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    protected CreateAndFakeException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>Integrates the details into the message.</summary>
    /// <param name="error">Starting message.</param>
    /// <param name="details">Details to integrate.</param>
    /// <param name="content">Content to integrate.</param>
    /// <returns>Message to use for the exception.</returns>
    protected static string BuildMessage(string error, string? details, string? content = null)
    {
        string nl = Environment.NewLine;

        return $"{nameof(CreateAndFake)} library exception."
            + $"{nl}Error: {error}"
            + ((details != null) ? $"{nl}Details: {details}" : "")
            + ((content != null) ? $"{nl}Content: {nl}{content}" : "");
    }
}
