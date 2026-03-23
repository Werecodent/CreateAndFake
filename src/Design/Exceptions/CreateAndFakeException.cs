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
}
