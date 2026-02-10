using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for errors occurring
///     due to accessing asynchronous behavior in synchronous context.
/// </summary>
[Serializable]
public sealed class AsynchronousAccessException : CreateAndFakeException
{
    /// <inheritdoc cref="AsynchronousAccessException"/>
    /// <remarks>Serialization constructor.</remarks>
    private AsynchronousAccessException()
        : base() { }

    /// <inheritdoc cref="AsynchronousAccessException"/>
    /// <inheritdoc/>
    public AsynchronousAccessException(string? message)
        : base(message) { }

    /// <inheritdoc cref="AsynchronousAccessException"/>
    /// <inheritdoc/>
    public AsynchronousAccessException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private AsynchronousAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
