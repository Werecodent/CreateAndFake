using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertError : AssertErrorBase<AssertError>
{
    /// <inheritdoc/>
    internal AssertError(IAsserter asserter, Exception? exception)
        : base(asserter, exception) { }
}
