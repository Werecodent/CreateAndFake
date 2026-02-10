using System.Runtime.Serialization;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.FakerTool.Proxy;

/// <summary>Exception for call counts not matching expectations.</summary>
[Serializable]
public sealed class FakeVerifyException : CreateAndFakeException
{
    /// <inheritdoc cref="FakeVerifyException"/>
    /// <remarks>Serialization constructor.</remarks>
    private FakeVerifyException()
        : base() { }

    /// <inheritdoc cref="FakeVerifyException"/>
    /// <param name="data">Associated call data.</param>
    /// <param name="expected">Expected number of calls.</param>
    /// <param name="actual">Actual number of calls.</param>
    /// <param name="log">Log of all made calls.</param>
    internal FakeVerifyException(
        CallData data,
        Times expected,
        int actual,
        IEnumerable<CallData> log
    )
        : base(BuildMessage(data, expected?.ToString(), actual, log)) { }

    /// <inheritdoc cref="FakeVerifyException"/>
    /// <param name="expected">Expected number of calls.</param>
    /// <param name="log">Log of all made calls.</param>
    internal FakeVerifyException(Times expected, IEnumerable<CallData> log)
        : base(BuildMessage(null, expected?.ToString(), log?.Count(), log)) { }

    /// <inheritdoc cref="FakeVerifyException"/>
    /// <param name="invalid">Call data with behavior that aren't valid.</param>
    /// <param name="log">Log of all made calls.</param>
    internal FakeVerifyException(
        IEnumerable<(CallData, Behavior)> invalid,
        IEnumerable<CallData> log
    )
        : base(
            string.Join(
                Environment.NewLine,
                invalid
                    .Select(i =>
                        BuildMessage(i.Item1, i.Item2.ToExpectedCalls(), i.Item2.Calls, null)
                    )
                    .Append(BuildMessage(log))
            )
        ) { }

    /// <inheritdoc cref="FakeVerifyException"/>
    /// <param name="info">Object data.</param>
    /// <param name="context">Additional info.</param>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private FakeVerifyException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>Integrates the details into the message.</summary>
    /// <param name="source">Associated call data.</param>
    /// <param name="expected">Expected number of calls.</param>
    /// <param name="actual">Actual number of calls.</param>
    /// <param name="log">Log of all made calls.</param>
    /// <returns>Message to use for the exception.</returns>
    private static string BuildMessage(
        CallData? source,
        string? expected,
        int? actual,
        IEnumerable<CallData>? log
    )
    {
        string nl = Environment.NewLine;

        return $"Expected '{expected}', but had '{actual}' calls."
            + ((source == null) ? "" : $"{nl}Call: {source}")
            + ((log == null) ? "" : $"{nl}Log: {BuildMessage(log)}");
    }

    /// <summary>Integrates the details into the message.</summary>
    /// <param name="log">Log of all made calls.</param>
    /// <returns>Message to use for the exception.</returns>
    private static string BuildMessage(IEnumerable<CallData> log)
    {
        return Environment.NewLine + string.Join(Environment.NewLine, log);
    }
}
