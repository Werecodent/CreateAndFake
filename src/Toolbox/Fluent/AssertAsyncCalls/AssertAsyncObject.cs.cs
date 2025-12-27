using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertAsyncObject : AssertAsyncObjectBase<AssertAsyncObject>
{
    /// <inheritdoc/>
    internal AssertAsyncObject(IAsserter asserter, object? actual)
        : base(asserter, actual) { }
}
