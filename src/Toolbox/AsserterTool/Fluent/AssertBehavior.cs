namespace CreateAndFake.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertBehavior : AssertBehaviorBase<AssertBehavior>
{
    /// <inheritdoc/>
    internal AssertBehavior(IAsserter asserter, Delegate? behavior)
        : base(asserter, behavior) { }
}
