using System.Runtime.Serialization;

namespace Werecodent.CreateAndFake.Design.Exceptions;

/// <summary><see cref="Exception"/> <see cref="Type"/> for when the Runner tool times out.</summary>
[Serializable]
public sealed class RunnerTimeoutException : CreateAndFakeException
{
    /// <inheritdoc cref="RunnerTimeoutException"/>
    /// <remarks>Serialization constructor.</remarks>
    private RunnerTimeoutException()
        : base() { }

    /// <inheritdoc cref="RunnerTimeoutException"/>
    public RunnerTimeoutException(TimeSpan timeout, string method)
        : base(
            BuildMessage(
                "Attempting to run a method timed out.",
                $"Timed out after {timeout}.",
                method
            )
        ) { }

    //$"Attempting to run method '{GenericConverter.BuildTestName(data.Method)}' timed out: {timeout}"

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private RunnerTimeoutException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
