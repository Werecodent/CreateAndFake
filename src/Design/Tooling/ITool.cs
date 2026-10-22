namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Reflection tool.</summary>
/// <typeparam name="TOptions">The configuration <see cref="Type"/> for the tool.</typeparam>
public interface ITool<out TOptions>
    where TOptions : IToolOptions
{
    /// <summary>Configured options for manipulating tool behavior.</summary>
    TOptions Options { get; }
}
