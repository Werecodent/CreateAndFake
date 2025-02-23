using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public record ValuerOptions : IToolOptions
{
    /// <summary>If the default set of hints should be used in comparison.</summary>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <summary>Custom comparators used to compare specific types.</summary>
    public ImmutableArray<CompareHint> Hints { get; init; } = [];

    /// <summary>Allows <see cref="IEquatable{T}"/> to handle comparisons if applicable.</summary>
    public bool UseEquatableComparisons { get; init; } = true;

    /// <summary>Triggers type checking for collections.</summary>
    /// <remarks>By default, collections are compared by contents and not the container type.</remarks>
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Types to use default equality/hashing.</summary>
    public FrozenSet<Type> FallbackTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);
}
