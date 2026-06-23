using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertGenericTask<T> : AssertGenericTaskBase<T, AssertGenericTask<T>>
{
    /// <inheritdoc/>
    internal AssertGenericTask(IAsserter asserter, Task<T>? behavior)
        : base(asserter, behavior) { }
}
