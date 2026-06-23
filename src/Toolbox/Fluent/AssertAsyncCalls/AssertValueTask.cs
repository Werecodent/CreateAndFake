using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertValueTask : AssertValueTaskBase<AssertValueTask>
{
    /// <inheritdoc/>
    internal AssertValueTask(IAsserter asserter, ValueTask? operation)
        : base(asserter, operation) { }
}
