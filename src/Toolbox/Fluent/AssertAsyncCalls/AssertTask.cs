using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertTask : AssertTaskBase<AssertTask>
{
    /// <inheritdoc/>
    internal AssertTask(IAsserter asserter, Task? operation)
        : base(asserter, operation) { }
}
