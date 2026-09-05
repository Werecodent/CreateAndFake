using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

#pragma warning disable CA1711 // Follows existing pattern.

/// <inheritdoc/>
public sealed class AssertDelegate : AssertDelegateBase<AssertDelegate>
{
    /// <inheritdoc/>
    internal AssertDelegate(IAsserter asserter, Delegate? behavior)
        : base(asserter, behavior) { }
}

#pragma warning restore
