using System.Collections.Immutable;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tooling;

/// <inheritdoc cref="IToolHintOptions{T,T}"/>
public abstract record ToolHintOptions<TSelf, THint> : IToolHintOptions<TSelf, THint>
    where TSelf : IToolHintOptions<TSelf, THint>
    where THint : IToolHint
{
    /// <inheritdoc/>
    public required IRandom Gen { get; init; }

    /// <inheritdoc/>
    [ConfigurableOption]
    public bool IncludeFrameworkHints { get; init; } = true;

    /// <inheritdoc/>
    [ConfigurableOption]
    public bool IncludeFoundHints { get; init; } = true;

    /// <inheritdoc/>
    [ConfigurableOption]
    public int MaxHintRecursion { get; init; } = 30;

    /// <inheritdoc/>
    public ImmutableArray<THint> Hints { get; init; } = [];

    /// <inheritdoc/>
    public TSelf? NestedOptions { get; init; } = default;

    /// <inheritdoc/>
    public sealed override string ToString()
    {
        // Prevents infinite loop from nested options.
        return TypeHelper.ExpandedName(GetType());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        // Prevents infinite loop from nested options.
        return ValueComparer.Use.GetHashCode(
            Gen,
            IncludeFrameworkHints,
            IncludeFoundHints,
            MaxHintRecursion
        );
    }
}
