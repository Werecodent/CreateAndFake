namespace CreateAndFake.Design.Tooling;

#pragma warning disable S2326 // Marker for inheritance.

/// <summary>Reflection tool that utilizes <see cref="IToolHint"/>s.</summary>
/// <inheritdoc/>
public interface IHintTool<TOptions, THint> : ITool<TOptions>
    where TOptions : IToolHintOptions<TOptions, THint>
    where THint : IToolHint
{
    /// <inheritdoc cref="IToolEngine{T}.SupportedTypes"/>
    IEnumerable<Type> SupportedTypes { get; }
}

#pragma warning restore S2326
