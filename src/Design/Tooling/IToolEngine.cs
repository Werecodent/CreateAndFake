namespace CreateAndFake.Design.Tooling;

#pragma warning disable S2326 // Marker for inheritance.

/// <summary>Handles the <typeparamref name="THint"/> behavior pipeline for the tool.</summary>
/// <typeparam name="THint">Hint <see cref="Type"/> being used by the tool.</typeparam>
public interface IToolEngine<out THint>
    where THint : IToolHint
{
    /// <inheritdoc cref="IToolHint.SupportedTypes"/>
    IEnumerable<Type> SupportedTypes { get; }
}

#pragma warning restore S2326
