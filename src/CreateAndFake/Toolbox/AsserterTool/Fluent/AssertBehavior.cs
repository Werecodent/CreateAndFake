namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertBehavior : AssertBehaviorBase<AssertBehavior>
{
    /// <inheritdoc/>
    internal AssertBehavior(AsserterOptions options, Delegate? behavior) : base(options, behavior) { }
}
