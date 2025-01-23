namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertObject : AssertObjectBase<AssertObject>
{
    /// <inheritdoc/>
    internal AssertObject(IAsserter asserter, object? actual) : base(asserter, actual) { }
}
