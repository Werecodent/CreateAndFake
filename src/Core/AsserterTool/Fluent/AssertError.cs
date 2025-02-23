namespace CreateAndFake.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertError : AssertErrorBase<AssertError>
{
    /// <inheritdoc/>
    internal AssertError(IAsserter asserter, Exception? error) : base(asserter, error) { }
}
