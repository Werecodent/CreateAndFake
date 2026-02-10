namespace CreateAndFake.Design.Tooling;

/// <summary>Reflection tool.</summary>
/// <typeparam name="TOptions">The configuration <see cref="Type"/> for the tool.</typeparam>
public interface ITool<out TOptions>
    where TOptions : IToolOptions
{
    /// <summary>Configured options being used by the tool.</summary>
    TOptions Options { get; }
}
