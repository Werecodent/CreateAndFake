using System.Collections.Frozen;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public sealed record ValuerOptions : ToolHintOptions<ValuerOptions, CompareHint>
{
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

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(
            IncludeDefaultHints,
            Hints,
            UseEquatableComparisons,
            CheckCollectionType,
            IgnoreCurrentRandomSeed,
            FallbackTypes,
            AsyncTimeout,
            SkipAsyncValues
        );
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(ValuerOptions);
    }
}
