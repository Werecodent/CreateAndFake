using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertType : AssertTypeBase<AssertType>
{
    /// <inheritdoc/>
    internal AssertType(IAsserter asserter, Type? type)
        : base(asserter, type) { }
}
