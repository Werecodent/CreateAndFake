namespace CreateAndFake.Design.Tooling;

#pragma warning disable S2326 // Marker for inheritance.

/// <summary>Runs the hint behavior pipeline.</summary>
/// <typeparam name="THint">Hint type being used.</typeparam>
public interface IToolEngine<out THint>
    where THint : IToolHint
{
    /// <summary>Specific types that the hint can support.</summary>
    /// <remarks>Not inclusive and not required.</remarks>
    IEnumerable<Type> SupportedTypes { get; }
}

#pragma warning restore S2326
