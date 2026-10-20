using System.Collections.Frozen;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Settings for manipulating <see cref="IExtractor"/> behavior.</summary>
public sealed record ExtractorOptions : ToolHintOptions<ExtractorOptions, IExtractHint>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Extractor);

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <summary>
    ///     If non-<see langword="public"/> properties/fields should be included while extracting contents.
    /// </summary>
    /// <remarks>
    ///     Only <see langword="public"/> properties/fields are extracted when not set to <see langword="true"/>.
    /// </remarks>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool ExtractPrivateMembers { get; init; } = false;

    /// <summary><see cref="Type"/>s to exclude when determining shared content among extracted content maps.</summary>
    /// <remarks>Represents <see cref="Type"/>s with too small of a range for unique randomization.</remarks>
    /// <value><c>[typeof(bool), typeof(byte), typeof(char)]</c></value>
    public FrozenSet<Type> IgnoredTypesForUniques { get; init; } =
        FrozenSet.ToFrozenSet([typeof(bool), typeof(byte), typeof(char)]);

    /// <summary><see cref="Type"/>s that are extracted as-is when extracting contents.</summary>
    /// <remarks>
    ///     No internal members (<see langword="public"/> nor <see langword="private"/>)
    ///     are included when extracting these <see cref="Type"/>s.
    /// </remarks>
    /// <value><c>[]</c></value>
    public FrozenSet<Type> NonExtractableTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal ExtractorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            ExtractPrivateMembers = Config.GetValue(section, ExtractPrivateMembers),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
        };
    }
}
