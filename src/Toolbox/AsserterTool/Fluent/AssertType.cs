namespace CreateAndFake.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertType : AssertTypeBase<AssertType>
{
    /// <inheritdoc/>
    internal AssertType(IAsserter asserter, Type? type)
        : base(asserter, type) { }
}
