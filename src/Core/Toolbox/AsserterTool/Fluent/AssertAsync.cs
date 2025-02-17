namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertAsync : AssertAsyncBase<AssertAsync>
{
    /// <inheritdoc/>
    internal AssertAsync(IAsserter asserter, Func<Task?>? behavior) : base(asserter, behavior) { }
}
