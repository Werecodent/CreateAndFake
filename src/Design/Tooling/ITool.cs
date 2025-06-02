namespace CreateAndFake.Design.Tooling;

/// <summary>Test</summary>
/// <typeparam name="TOptions"></typeparam>
public interface ITool<out TOptions>
    where TOptions : IToolOptions
{
    /// <summary>Configured options being used by the tool.</summary>
    TOptions Options { get; }
}
