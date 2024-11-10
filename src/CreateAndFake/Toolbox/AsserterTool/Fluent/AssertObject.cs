namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertObject : AssertObjectBase<AssertObject>
{
    /// <inheritdoc/>
    internal AssertObject(AsserterOptions options, object? actual) : base(options, actual) { }
}
