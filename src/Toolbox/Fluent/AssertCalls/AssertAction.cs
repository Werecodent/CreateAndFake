using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertAction : AssertActionBase<AssertAction>
{
    /// <inheritdoc/>
    internal AssertAction(IAsserter asserter, Action? behavior)
        : base(asserter, behavior) { }
}
