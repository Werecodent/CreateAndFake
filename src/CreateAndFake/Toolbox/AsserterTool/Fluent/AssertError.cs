namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <inheritdoc/>
public sealed class AssertError : AssertErrorBase<AssertError>
{
    /// <inheritdoc/>
    internal AssertError(AsserterOptions options, Exception? error) : base(options, error) { }
}

