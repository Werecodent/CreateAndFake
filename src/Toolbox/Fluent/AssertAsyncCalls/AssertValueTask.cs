using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertValueTask : AssertValueTaskBase<AssertValueTask>
{
    /// <inheritdoc/>
    internal AssertValueTask(IAsserter asserter, ValueTask? operation)
        : base(asserter, operation) { }
}
