using CreateAndFake.AsserterTool;

namespace CreateAndFake.Fluent.AssertCalls;

/// <inheritdoc/>
public sealed class AssertString : AssertStringBase<AssertString>
{
    /// <inheritdoc/>
    internal AssertString(IAsserter asserter, string? text)
        : base(asserter, text) { }
}
