namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertComparable : AssertComparableBase<AssertComparable>
{
    /// <inheritdoc/>
    internal AssertComparable(IAsserter asserter, IComparable? value) : base(asserter, value) { }
}
