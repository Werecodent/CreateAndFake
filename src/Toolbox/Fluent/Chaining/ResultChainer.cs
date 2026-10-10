using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

#pragma warning disable CA1024 // Hurts readability.

/// <summary>Chainer enabling additional assertion calls for resulting data.</summary>
/// <typeparam name="T">Result <see cref="Type"/> to chain.</typeparam>
/// <param name="result">Assertion base instance to chain.</param>
/// <inheritdoc cref="AlsoChainer(IAsserter)"/>
public sealed class ResultChainer<T>(T result, IAsserter asserter)
    : WithChainer<T>(result, asserter)
{
    /// <summary>Result returned.</summary>
    public T GetResultValue()
    {
        return Result;
    }
}

#pragma warning restore
