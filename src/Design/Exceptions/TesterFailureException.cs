using System.Runtime.Serialization;

namespace CreateAndFake.Design.Exceptions;

/// <summary><see cref="Exception"/> <see cref="Type"/> for errors found via the Tester tool.</summary>
[Serializable]
public sealed class TesterFailureException : CreateAndFakeException
{
    /// <inheritdoc cref="TesterFailureException"/>
    /// <remarks>Serialization constructor.</remarks>
    private TesterFailureException()
        : base() { }

    /// <inheritdoc cref="TesterFailureException"/>
    /// <inheritdoc/>
    public TesterFailureException(string? message)
        : base(BuildMessage("Encountered fatal Tester issue.", message)) { }

    /// <inheritdoc cref="TesterFailureException"/>
    /// <inheritdoc/>
    public TesterFailureException(string? message, Exception? innerException)
        : base(BuildMessage("Encountered fatal Tester exception.", message), innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private TesterFailureException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
