using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertAction : AssertActionBase<AssertAction>
{
    /// <inheritdoc/>
    internal AssertAction(IAsserter asserter, Action? behavior)
        : base(asserter, behavior) { }
}
