using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

#pragma warning disable CA1024 // Hurts readability.

/// <summary>Chainer enabling additional assertion calls on the result.</summary>
/// <inheritdoc/>
public sealed class ResultChainer<T>(T result, IAsserter asserter)
    : WithChainer<T>(result, asserter)
{
    /// <summary>Result instance to chain with more assertions.</summary>
    public T GetResultValue()
    {
        return Result;
    }
}

#pragma warning restore
