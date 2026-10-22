using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertObject : AssertObjectBase<AssertObject>
{
    /// <inheritdoc/>
    internal AssertObject(IAsserter asserter, object? actual)
        : base(asserter, actual) { }
}
