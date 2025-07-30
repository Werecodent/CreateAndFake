using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertComparable : AssertComparableBase<AssertComparable>
{
    /// <inheritdoc/>
    internal AssertComparable(IAsserter asserter, IComparable? value)
        : base(asserter, value) { }
}
