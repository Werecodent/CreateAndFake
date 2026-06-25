using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Fluent.Chaining;

#pragma warning disable CA1024 // Hurts readability.

/// <summary>Chainer enabling additional assertion calls for resulting data.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="result">Assertion base instance to chain.</param>
/// <inheritdoc cref="AlsoChainer(IAsserter)"/>
public sealed class ResultChainer<T>(T result, IAsserter asserter) : AlsoChainer(asserter)
{
    /// <summary>Includes another assertion on the instance to test.</summary>
    public AssertAsyncObject That => Also(result);

    /// <summary>Exception returned.</summary>
    public T With => result;

    /// <summary>Result returned.</summary>
    public T GetResultValue()
    {
        return result;
    }
}

#pragma warning restore
