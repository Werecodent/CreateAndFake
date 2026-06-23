using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for awaiting a
///     <see cref="ValueTask"/> or <see cref="ValueTask{T}"/> multiple times.
/// </summary>
[Serializable]
public sealed class ValueTaskRepeatedAccessException : CreateAndFakeException
{
    /// <inheritdoc cref="ValueTaskRepeatedAccessException"/>
    /// <remarks>Serialization constructor.</remarks>
    private ValueTaskRepeatedAccessException()
        : base() { }

    /// <inheritdoc cref="ValueTaskRepeatedAccessException"/>
    /// <inheritdoc/>
    public ValueTaskRepeatedAccessException(string? message)
        : base(
            "Resolving a ValueTask multiple times results in undefined behavior."
                + message?.PadLeft(1)
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private ValueTaskRepeatedAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
