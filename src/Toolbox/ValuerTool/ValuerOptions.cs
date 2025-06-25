using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public record ValuerOptions : IToolHintOptions<ValuerOptions, CompareHint>
{
    /// <inheritdoc/>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <inheritdoc/>
    public ImmutableArray<CompareHint> Hints { get; init; } = [];

    /// <inheritdoc/>
    public ValuerOptions? NestedOptions => null;

    /// <summary>Allows <see cref="IEquatable{T}"/> to handle comparisons if applicable.</summary>
    public bool UseEquatableComparisons { get; init; } = true;

    /// <summary>Triggers type checking for collections.</summary>
    /// <remarks>By default, collections are compared by contents and not the container type.</remarks>
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Excludes <see cref="SeededRandom.Seed"/> from comparison checks.</summary>
    public bool IgnoreCurrentRandomSeed { get; init; } = true;

    /// <summary>Types to use default equality/hashing.</summary>
    public FrozenSet<Type> FallbackTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>How long to wait for async comparisons to complete.</summary>
    public TimeSpan AsyncTimeout { get; init; } = new(0, 0, 5);

    /// <summary>If asynchronous values should be skipped in synchronous contexts instead of throwing.</summary>
    public bool SkipAsyncValues { get; init; } = false;
}
