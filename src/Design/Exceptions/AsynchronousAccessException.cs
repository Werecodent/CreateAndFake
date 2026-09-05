using System.Runtime.Serialization;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Exceptions;

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

    /// <param name="value">Asynchronous instance attempting to be accessed.</param>
    /// <param name="message">Details to include in the error message.</param>
    /// <inheritdoc cref="AsynchronousAccessException"/>
    public AsynchronousAccessException(object? value, string? message)
        : base(
            BuildMessage(
                $"Prevented synchronous access to instance of type '{GenericConverter.ExpandName(value)}'.",
                message,
                value?.ToString()
            )
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private AsynchronousAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
