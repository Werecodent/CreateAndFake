using System.Collections.Frozen;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool.Engine;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.ExtractorTool;

/// <summary>Configuration for controlling extraction behavior.</summary>
public sealed record ExtractorOptions : ToolHintOptions<ExtractorOptions, IExtractHint>
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>If private properties/fields should be extracted as well.</summary>
    [ConfigurableOption]
    public bool ExtractPrivateMembers { get; init; } = false;

    /// <summary>Types with too small of range for unique randomization.</summary>
    public FrozenSet<Type> UniqueIgnoredTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(bool), typeof(byte), typeof(char)]);

    /// <summary>Types that need no further inspection when creating a <see cref="ContentMap"/>.</summary>
    public FrozenSet<Type> ContentEndTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal ExtractorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Extractor));
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
            ExtractPrivateMembers = section.GetValue(
                nameof(ExtractPrivateMembers),
                ExtractPrivateMembers
            ),
        };
    }
}
