namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertType : AssertTypeBase<AssertType>
{
    /// <inheritdoc/>
    internal AssertType(AsserterOptions options, Type? type) : base(options, type) { }
}
