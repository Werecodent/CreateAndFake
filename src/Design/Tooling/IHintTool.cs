namespace CreateAndFake.Design.Tooling;

/// <summary>Reflection tool that utilizes hints.</summary>
/// <inheritdoc/>
public interface IHintTool<TOptions> : ITool<TOptions>
    where TOptions : IToolOptions
{
    /// <summary>Specific types that the hint can support.</summary>
    /// <remarks>Not inclusive and not required.</remarks>
    IEnumerable<Type> SupportedTypes { get; }
}
