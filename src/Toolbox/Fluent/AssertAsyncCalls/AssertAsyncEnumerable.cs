using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

/// <inheritdoc/>
public sealed class AssertAsyncEnumerable<T>
    : AssertAsyncEnumerableBase<T, AssertAsyncEnumerable<T>>
{
    /// <inheritdoc/>
    internal AssertAsyncEnumerable(IAsserter asserter, IAsyncEnumerable<T>? collection)
        : base(asserter, collection) { }
}
