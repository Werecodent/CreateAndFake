using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertAsyncObject : AssertAsyncObjectBase<AssertAsyncObject>
{
    /// <inheritdoc/>
    internal AssertAsyncObject(IAsserter asserter, object? actual)
        : base(asserter, actual) { }
}
