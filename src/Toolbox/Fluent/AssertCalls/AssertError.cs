using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertError : AssertErrorBase<AssertError>
{
    /// <inheritdoc/>
    internal AssertError(IAsserter asserter, Exception? error)
        : base(asserter, error) { }
}
