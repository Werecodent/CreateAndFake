using System.Runtime.Serialization;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Exceptions;

/// <summary>
///     <see cref="Exception"/> <see cref="Type"/> for errors occurring due
///     to trying to extract from a different context than the initial one.
/// </summary>
[Serializable]
public sealed class MismatchedAccessException : CreateAndFakeException
{
    /// <inheritdoc cref="MismatchedAccessException"/>
    /// <remarks>Serialization constructor.</remarks>
    private MismatchedAccessException()
        : base() { }

    /// <param name="value">Instance attempting to be added.</param>
    /// <param name="message">Details to include in the error message.</param>
    /// <inheritdoc cref="MismatchedAccessException"/>
    public MismatchedAccessException(object? value, string? message)
        : base(
            BuildMessage(
                $"Prevented access to instance of type '{GenericConverter.ExpandName(value)}'.",
                message,
                value?.ToString()
            )
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private MismatchedAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
