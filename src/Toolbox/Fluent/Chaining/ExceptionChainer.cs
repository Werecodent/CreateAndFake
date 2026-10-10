using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

#pragma warning disable CA1024 // Hurts readability.

/// <inheritdoc/>
public sealed class ExceptionChainer<T>(T result, IAsserter asserter)
    : WithChainer<T>(result, asserter)
    where T : Exception
{
    /// <summary>Includes another assertion on the instance to test.</summary>
    public AssertError That()
    {
        return Also(Result);
    }

    /// <summary>Exception returned.</summary>
    public T GetCaughtException()
    {
        return Result;
    }
}

#pragma warning restore
