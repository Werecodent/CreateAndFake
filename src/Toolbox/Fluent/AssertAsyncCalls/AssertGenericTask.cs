using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertGenericTask<T> : AssertGenericTaskBase<T, AssertGenericTask<T>>
{
    /// <inheritdoc/>
    internal AssertGenericTask(IAsserter asserter, Task<T>? behavior)
        : base(asserter, behavior) { }
}
