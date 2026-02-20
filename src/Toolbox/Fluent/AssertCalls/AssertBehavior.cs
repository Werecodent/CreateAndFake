using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertBehavior : AssertBehaviorBase<AssertBehavior>
{
    /// <inheritdoc/>
    internal AssertBehavior(IAsserter asserter, Delegate? behavior)
        : base(asserter, behavior) { }
}
