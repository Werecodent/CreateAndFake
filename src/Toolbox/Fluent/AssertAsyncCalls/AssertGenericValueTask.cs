using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertGenericValueTask<T>
    : AssertGenericValueTaskBase<T, AssertGenericValueTask<T>>
{
    /// <inheritdoc/>
    internal AssertGenericValueTask(IAsserter asserter, ValueTask<T>? operation)
        : base(asserter, operation) { }
}
