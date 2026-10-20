using System.Collections.Frozen;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool;

/// <summary>Configuration for controlling comparison behavior.</summary>
public sealed record ValuerOptions : ToolHintOptions<ValuerOptions, ICompareHint>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Valuer);

    /// <summary>Allows <see cref="IEquatable{T}"/> to handle comparisons if applicable.</summary>
    [ConfigurableOption]
    public bool UseEquatableComparisons { get; init; } = false;

    /// <summary>Triggers type checking for collections.</summary>
    /// <remarks>By default, collections are compared by contents and not the container type.</remarks>
    [ConfigurableOption]
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Excludes <see cref="SeededRandom.Seed"/> from comparison checks.</summary>
    [ConfigurableOption]
    public bool IgnoreCurrentRandomSeed { get; init; } = true;

    /// <summary>Types to use default equality/hashing.</summary>
    //[ConfigurableOption]
    public FrozenSet<Type> DefaultEqualityTypes { get; init; } = [typeof(ValueTuple)];

    /// <summary>How long to wait for async comparisons to complete.</summary>
    [ConfigurableOption]
    public TimeSpan AsyncTimeout { get; init; } = new(0, 1, 0);

    /// <summary>If asynchronous values should be skipped in synchronous contexts instead of throwing.</summary>
    [ConfigurableOption]
    public bool SkipAsyncValues { get; init; } = false;

    /// <summary>If calculated value hashes should be included in equality comparisons.</summary>
    [ConfigurableOption]
    public bool IncludeValueHashInComparison { get; init; } = false;

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal ValuerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeValueHashInComparison = Config.GetValue(section, IncludeValueHashInComparison),
            IgnoreCurrentRandomSeed = Config.GetValue(section, IgnoreCurrentRandomSeed),
            UseEquatableComparisons = Config.GetValue(section, UseEquatableComparisons),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            CheckCollectionType = Config.GetValue(section, CheckCollectionType),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
            SkipAsyncValues = Config.GetValue(section, SkipAsyncValues),
            AsyncTimeout = Config.GetValue(section, AsyncTimeout),
        };
    }
}
