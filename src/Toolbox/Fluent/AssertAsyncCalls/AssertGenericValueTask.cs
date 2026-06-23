using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertGenericValueTask<T>
    : AssertGenericValueTaskBase<T, AssertGenericValueTask<T>>
{
    /// <inheritdoc/>
    internal AssertGenericValueTask(IAsserter asserter, ValueTask<T>? operation)
        : base(asserter, operation) { }
}
