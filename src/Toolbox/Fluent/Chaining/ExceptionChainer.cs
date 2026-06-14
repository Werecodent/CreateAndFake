using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls for throws assertions.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="exception">Assertion base instance to chain.</param>
/// <inheritdoc cref="AlsoChainer(IAsserter)"/>
public sealed class ExceptionChainer<T>(T exception, IAsserter asserter) : AlsoChainer(asserter)
    where T : Exception
{
    /// <summary>Includes another assertion on the instance to test.</summary>
    public AssertError That { get; } = exception.Assert();

    /// <summary>Exception returned.</summary>
    public T With => exception;
}
