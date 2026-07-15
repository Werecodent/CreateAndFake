using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>Default <see cref="Exception"/> <see cref="Type"/> when throwing via set Behavior for fakes.</summary>
[Serializable]
public sealed class BehaviorDefaultThrowException : CreateAndFakeException
{
    /// <inheritdoc cref="BehaviorDefaultThrowException"/>
    /// <remarks>Serialization constructor.</remarks>
    private BehaviorDefaultThrowException()
        : base() { }

    /// <inheritdoc cref="BehaviorDefaultThrowException"/>
    /// <inheritdoc/>
    public BehaviorDefaultThrowException(string? message)
        : base(
            BuildMessage(
                "Behavior called that has been configured to throw a general exception.",
                message
            )
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private BehaviorDefaultThrowException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
