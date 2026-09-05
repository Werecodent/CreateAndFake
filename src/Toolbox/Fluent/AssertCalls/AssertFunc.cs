using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertFunc<T> : AssertFuncBase<T, AssertFunc<T>>
{
    /// <inheritdoc/>
    internal AssertFunc(IAsserter asserter, Func<T>? behavior)
        : base(asserter, behavior) { }
}
