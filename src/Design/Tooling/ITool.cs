namespace CreateAndFake.Design.Tooling;

/// <summary></summary>
/// <typeparam name="TOptions"></typeparam>
public interface ITool<TOptions>
    where TOptions : IToolOptions
{
    /// <summary>Configured options being used by the tool.</summary>
    TOptions Options { get; }
}
