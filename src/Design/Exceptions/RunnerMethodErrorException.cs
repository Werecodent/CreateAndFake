using System.Runtime.Serialization;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Exceptions;

/// <summary><see cref="Exception"/> <see cref="Type"/> for when the Runner tool encounters an exception.</summary>
[Serializable]
public sealed class RunnerMethodErrorException : CreateAndFakeException
{
    /// <inheritdoc cref="RunnerMethodErrorException"/>
    /// <remarks>Serialization constructor.</remarks>
    private RunnerMethodErrorException()
        : base() { }

    /// <inheritdoc cref="RunnerMethodErrorException"/>
    public RunnerMethodErrorException(string method, Exception innerException)
        : base(
            BuildMessage(
                "Running a method failed.",
                $"Encountered an exception of type '{GenericConverter.ExpandName(innerException)}'.",
                method
            ),
            innerException
        ) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private RunnerMethodErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
