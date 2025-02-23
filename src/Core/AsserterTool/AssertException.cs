using System.Runtime.Serialization;

namespace CreateAndFake.AsserterTool;

/// <summary>Exception for a failed <see cref="IAsserter"/> case.</summary>
[Serializable, KnownType(typeof(Exception)), KnownType(typeof(Exception[]))]
public sealed class AssertException : Exception
{
    /// <inheritdoc cref="AssertException"/>
    /// <remarks>Serialization constructor.</remarks>
    private AssertException() : base() { }

    /// <inheritdoc cref="AssertException"/>
    /// <param name="message">Reason for the exception.</param>
    /// <param name="details">Optional fail details.</param>
    /// <param name="seed">Seed used for data generation.</param>
    /// <param name="content">Optional related content.</param>
    public AssertException(string? message, string? details, int? seed, string? content = null)
        : base(BuildMessage(message, details, seed, content)) { }

    /// <inheritdoc cref="AssertException"/>
    /// <param name="message">Reason for the exception.</param>
    /// <param name="details">Optional fail details.</param>
    /// <param name="seed">Seed used for data generation.</param>
    /// <param name="innerException">Inner exception that occurred.</param>
    public AssertException(string? message, string? details, int? seed, Exception? innerException)
        : base(BuildMessage(message, details, seed), innerException) { }

    /// <inheritdoc cref="AssertException"/>
    /// <param name="message">Reason for the exception.</param>
    /// <param name="details">Optional fail details.</param>
    /// <param name="seed">Seed used for data generation.</param>
    /// <param name="content">Optional related content.</param>
    /// <param name="innerException">Inner exception that occurred.</param>
    public AssertException(string? message, string? details, int? seed, string? content, Exception? innerException)
        : base(BuildMessage(message, details, seed, content), innerException) { }

    /// <inheritdoc cref="AssertException"/>
    /// <param name="info">Object data.</param>
    /// <param name="context">Additional info.</param>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
    private AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>Integrates the details into the message.</summary>
    /// <param name="message">Starting message.</param>
    /// <param name="details">Details to integrate.</param>
    /// <param name="seed">Seed used for data generation.</param>
    /// <param name="content">Content to integrate.</param>
    /// <returns>Message to use for the exception.</returns>
    private static string BuildMessage(string? message, string? details, int? seed, string? content = null)
    {
        string nl = Environment.NewLine;

        return (message ?? "Unknown assert failure.") +
            ((details != null) ? $"{nl}Details: {details}" : "") +
            (seed.HasValue ? $"{nl}Seed: {seed.Value}" : "") +
            ((content != null) ? $"{nl}Content: {nl}{content}" : "");
    }
}
