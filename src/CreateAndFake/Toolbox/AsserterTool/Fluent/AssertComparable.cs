namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertComparable : AssertComparableBase<AssertComparable>
{
    /// <inheritdoc/>
    internal AssertComparable(AsserterOptions options, IComparable? value) : base(options, value) { }
}
