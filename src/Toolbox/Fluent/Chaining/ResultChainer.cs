using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls for resulting data.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="result">Assertion base instance to chain.</param>
/// <inheritdoc cref="AlsoChainer(IAsserter)"/>
public sealed class ResultChainer<T>(T result, IAsserter asserter) : AlsoChainer(asserter)
{
    /// <summary>Result returned.</summary>
    public T Result => result;
}
