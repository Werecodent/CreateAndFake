using System.Collections.Frozen;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public sealed record ValuerOptions : ToolHintOptions<ValuerOptions, ICompareHint>
{
    /// <summary>Allows <see cref="IEquatable{T}"/> to handle comparisons if applicable.</summary>
    [ConfigurableOption]
    public bool UseEquatableComparisons { get; init; } = true;

    /// <summary>Triggers type checking for collections.</summary>
    /// <remarks>By default, collections are compared by contents and not the container type.</remarks>
    [ConfigurableOption]
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Excludes <see cref="SeededRandom.Seed"/> from comparison checks.</summary>
    [ConfigurableOption]
    public bool IgnoreCurrentRandomSeed { get; init; } = true;

    /// <summary>Types to use default equality/hashing.</summary>
    //[ConfigurableOption]
    public FrozenSet<Type> DefaultEqualityTypes { get; init; } = [];

    /// <summary>How long to wait for async comparisons to complete.</summary>
    [ConfigurableOption]
    public TimeSpan AsyncTimeout { get; init; } = new(0, 0, 5);

    /// <summary>If asynchronous values should be skipped in synchronous contexts instead of throwing.</summary>
    [ConfigurableOption]
    public bool SkipAsyncValues { get; init; } = false;

    /// <summary>If calculated value hashes should be included in equality comparisons.</summary>
    [ConfigurableOption]
    public bool IncludeValueHashInComparison { get; init; } = true;

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal ValuerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Valuer));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeFrameworkHints = section.GetValue(
                nameof(IncludeFrameworkHints),
                IncludeFrameworkHints
            ),
            IncludeFoundHints = section.GetValue(nameof(IncludeFoundHints), IncludeFoundHints),
            MaxHintRecursion = section.GetValue(nameof(MaxHintRecursion), MaxHintRecursion),
            UseEquatableComparisons = section.GetValue(
                nameof(UseEquatableComparisons),
                UseEquatableComparisons
            ),
            CheckCollectionType = section.GetValue(
                nameof(CheckCollectionType),
                CheckCollectionType
            ),
            IgnoreCurrentRandomSeed = section.GetValue(
                nameof(IgnoreCurrentRandomSeed),
                IgnoreCurrentRandomSeed
            ),
            AsyncTimeout = section.GetValue(nameof(AsyncTimeout), AsyncTimeout),
            SkipAsyncValues = section.GetValue(nameof(SkipAsyncValues), SkipAsyncValues),
            IncludeValueHashInComparison = section.GetValue(
                nameof(IncludeValueHashInComparison),
                IncludeValueHashInComparison
            ),
        };
    }
}
