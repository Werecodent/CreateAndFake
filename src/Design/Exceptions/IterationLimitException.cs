using System.Runtime.Serialization;
using Werecodent.CreateAndFake.Design.Properties;

namespace Werecodent.CreateAndFake.Design.Exceptions;

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

    /// <param name="iterationLimit"><inheritdoc cref="DesignDefaults.IterationLimit" path="/summary"/></param>
    /// <inheritdoc cref="IterationLimitException"/>
    /// <inheritdoc cref="CreateAndFakeException(string)"/>
    public IterationLimitException(int iterationLimit, string? message)
        : base(
            BuildMessage(
                $"Execution stopped at iteration {iterationLimit} to prevent infinite recursion.",
                message
            )
        ) { }

    /// <inheritdoc/>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private IterationLimitException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
