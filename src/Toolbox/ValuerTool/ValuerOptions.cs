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

    /// <summary>Allows <see cref="IEquatable{T}"/> implementations to handle comparisons if applicable.</summary>
    /// <remarks>Useful for <see cref="Type"/>s with value based overrides that are otherwise not supported.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool UseEquatableComparisons { get; init; } = false;

    /// <summary>Triggers strict <see cref="Type"/> checking for collections.</summary>
    /// <remarks>
    ///     Collections are compared by their contents and not the
    ///     container <see cref="Type"/> unless set to <see langword="true"/>.
    /// </remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool CheckCollectionType { get; init; } = false;

    /// <summary>Excludes <see cref="SeededRandom.Seed"/> from comparison checks.</summary>
    /// <remarks>Prevents issues from the <see cref="SeededRandom.Seed"/> changing during comparison.</remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IgnoreCurrentRandomSeed { get; init; } = true;

    /// <summary><see cref="Type"/>s to use default equality/hashing.</summary>
    /// <remarks>Useful for <see cref="Type"/>s with value based overrides that are otherwise not supported.</remarks>
    /// <value><c>[typeof(ValueTuple)]</c></value>
    [ConfigurableOption]
    public FrozenSet<Type> DefaultEqualityTypes { get; init; } = [typeof(ValueTuple)];

    /// <summary>How long to wait for <see langword="async"/> comparisons to complete.</summary>
    /// <remarks>Used to gracefully fail tests instead of stalling endlessly.</remarks>
    /// <value><c>00:01:00.0000000</c></value>
    [ConfigurableOption]
    public TimeSpan AsyncTimeout { get; init; } = new(0, 1, 0);

    /// <summary>If asynchronous values should be skipped in synchronous contexts instead of throwing.</summary>
    /// <remarks>Useful for contexts that must be synchronous even at the potential cost of a false positive.</remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool SkipAsyncValues { get; init; } = false;

    /// <summary>If calculated value hashes should be included in equality comparisons.</summary>
    /// <remarks>Useful for validating hashing with the caveat that hash collisions can be false negatives.</remarks>
    /// <value><see langword="false"/></value>
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
            DefaultEqualityTypes = Config.GetTypes(section, DefaultEqualityTypes),
            CheckCollectionType = Config.GetValue(section, CheckCollectionType),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
            SkipAsyncValues = Config.GetValue(section, SkipAsyncValues),
            AsyncTimeout = Config.GetValue(section, AsyncTimeout),
        };
    }
}
