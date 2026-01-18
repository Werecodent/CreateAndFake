namespace CreateAndFake.Design.Tooling;

/// <summary>Hint for controlling tool behavior.</summary>
public interface IToolHint
{
    /// <summary>Specific types that the hint can support.</summary>
    /// <remarks>Not inclusive and not required.</remarks>
    IEnumerable<Type> SupportedTypes { get; }
}
