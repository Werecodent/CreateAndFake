using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary><see cref="Exception"/> <see cref="Type"/> for errors occurring due to lack of tooling support.</summary>
[Serializable]
public sealed class UnsupportedException : CreateAndFakeException
{
    /// <inheritdoc cref="UnsupportedException"/>
    /// <remarks>Serialization constructor.</remarks>
    private UnsupportedException()
        : base() { }

    /// <inheritdoc cref="UnsupportedException"/>
    /// <inheritdoc/>
    public UnsupportedException(string? message)
        : base(BuildMessage("Case not supported by the framework.", message)) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private UnsupportedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
