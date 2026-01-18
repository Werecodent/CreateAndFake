namespace CreateAndFake.Design.Tooling;

#pragma warning disable S2326 // Marker for inheritance.

/// <summary>Runs the hint behavior pipeline.</summary>
/// <typeparam name="THint">Hint type being used.</typeparam>
public interface IToolEngine<out THint>
    where THint : IToolHint
{
    /// <inheritdoc cref="IHintTool{T}.SupportedTypes"/>
    IEnumerable<Type> SupportedTypes { get; }
}

#pragma warning restore S2326
