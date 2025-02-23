namespace CreateAndFake.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertString : AssertStringBase<AssertString>
{
    /// <inheritdoc/>
    internal AssertString(IAsserter asserter, string? text) : base(asserter, text) { }
}
