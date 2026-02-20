using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for fatal
///     errors occurring within <see cref="CreateAndFake"/> tools.
/// </summary>
[Serializable]
public sealed class ToolException : CreateAndFakeException
{
    /// <inheritdoc cref="ToolException"/>
    /// <remarks>Serialization constructor.</remarks>
    private ToolException()
        : base() { }

    /// <inheritdoc cref="ToolException"/>
    /// <inheritdoc/>
    public ToolException(string? message)
        : base(message) { }

    /// <inheritdoc cref="ToolException"/>
    /// <inheritdoc/>
    public ToolException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private ToolException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
