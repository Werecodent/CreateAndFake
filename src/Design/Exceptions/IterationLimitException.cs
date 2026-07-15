using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for errors occurring
///     due trying to iterate past the configured iteration limit.
/// </summary>
[Serializable]
public sealed class IterationLimitException : CreateAndFakeException
{
    /// <inheritdoc cref="IterationLimitException"/>
    /// <remarks>Serialization constructor.</remarks>
    private IterationLimitException()
        : base() { }

    /// <inheritdoc cref="IterationLimitException"/>
    public IterationLimitException(int iterationLimit, string? message)
        : base(
            BuildMessage(
                $"Execution stopped at iteration {iterationLimit} to prevent infinite recursion.",
                message
            )
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private IterationLimitException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
