using System.Collections.Immutable;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tooling;

/// <inheritdoc cref="IToolHintOptions{T,T}"/>
public abstract record ToolHintOptions<TSelf, THint>
    : ToolOptions<TSelf>,
        IToolHintOptions<TSelf, THint>
    where TSelf : ToolOptions<TSelf>, IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <inheritdoc/>
    /// <value><c>30</c></value>
    [ConfigurableOption]
    public int MaxRecursionDepth { get; init; } = 30;

    /// <inheritdoc/>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeFrameworkHints { get; init; } = true;

    /// <inheritdoc/>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeFoundHints { get; init; } = true;

    /// <inheritdoc/>
    /// <value><c>[]</c></value>
    public ImmutableArray<THint> Hints { get; init; } = [];

    /// <inheritdoc/>
    /// <value><see langword="null"/></value>
    public TSelf? NestedOptions { get; init; } = default;

    /// <inheritdoc/>
    public sealed override string ToString()
    {
        // Prevents infinite loop from nested options.
        return GenericConverter.ExpandName(GetType());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        // Prevents infinite loop from nested options.
        return ValueComparer.Use.GetHashCode(
            Gen,
            IncludeFrameworkHints,
            IncludeFoundHints,
            MaxRecursionDepth
        );
    }
}
