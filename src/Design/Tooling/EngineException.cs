using System.Runtime.Serialization;

namespace CreateAndFake.Design.Tooling;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for fatal errors
///     occurring within <see cref="CreateAndFake"/> tool engines.
/// </summary>
[Serializable, KnownType(typeof(Exception))]
public sealed class EngineException : Exception
{
    /// <inheritdoc cref="EngineException"/>
    /// <remarks>Serialization constructor.</remarks>
    private EngineException()
        : base() { }

    /// <inheritdoc cref="EngineException"/>
    /// <inheritdoc/>
    public EngineException(string? message)
        : base(message) { }

    /// <inheritdoc cref="EngineException"/>
    /// <inheritdoc/>
    public EngineException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private EngineException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
