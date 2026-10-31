using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

#pragma warning disable CA1024 // Hurts readability.

/// <summary>Chainer enabling additional assertion calls on the exception.</summary>
/// <inheritdoc/>
public sealed class ExceptionChainer<T>(T result, IAsserter asserter)
    : WithChainer<T>(result, asserter)
    where T : Exception
{
    /// <summary>Provides assertions to test the exception.</summary>
    public AssertError That()
    {
        return Also(Result);
    }

    /// <summary>Result exception to chain with more assertions.</summary>
    public T GetCaughtException()
    {
        return Result;
    }
}

#pragma warning restore
