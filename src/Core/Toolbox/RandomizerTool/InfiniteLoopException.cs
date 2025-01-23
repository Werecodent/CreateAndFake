using System.Runtime.Serialization;

namespace CreateAndFake.Toolbox.RandomizerTool;

/// <summary>Exception when encountering infinite loops.</summary>
[Serializable, KnownType(typeof(Exception)), KnownType(typeof(Exception[]))]
public sealed class InfiniteLoopException : Exception
{
    /// <inheritdoc cref="InfiniteLoopException"/>
    /// <remarks>Serialization constructor.</remarks>
    private InfiniteLoopException() : base() { }

    /// <inheritdoc cref="InfiniteLoopException"/>
    /// <param name="lastType">Last <c>Type</c> attempted to be created.</param>
    /// <param name="history">Stack of created parents.</param>
    public InfiniteLoopException(Type lastType, IEnumerable<Type> history) : base(
        $"Already created type '{lastType?.FullName}'. History:"
        + Environment.NewLine
        + string.Join(Environment.NewLine, history.Select(t => t.FullName)))
    { }

    /// <inheritdoc cref="InfiniteLoopException"/>
    /// <param name="info">Object data.</param>
    /// <param name="context">Additional info.</param>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
    private InfiniteLoopException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
