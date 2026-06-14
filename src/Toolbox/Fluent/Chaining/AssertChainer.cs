using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="chain">Assertion base instance to chain.</param>
/// <inheritdoc cref="AlsoChainer(IAsserter)"/>
public sealed class AssertChainer<T>(T chain, IAsserter asserter) : AlsoChainer(asserter)
{
    /// <summary>Includes another assertion on the instance to test.</summary>
    public T And { get; } = chain;
}
