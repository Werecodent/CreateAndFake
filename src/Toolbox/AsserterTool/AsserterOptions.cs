using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.AsserterTool;

/// <summary>Configuration for controlling assert behavior.</summary>
public sealed record AsserterOptions : IToolOptions
{
    /// <inheritdoc/>
    public required IRandom Gen { get; init; }

    /// <summary>Handles context extraction for comparisons.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Handles comparisons for assertion checks.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Options to use when performing <see langword="string"/> comparison (such as ignoring case or symbols).</summary>
    [ConfigurableOption]
    public StringComparison StringCompareOption { get; init; } = StringComparison.InvariantCulture;

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal AsserterOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Asserter));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            StringCompareOption = section.GetValue(
                nameof(StringCompareOption),
                StringCompareOption
            ),
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(AsserterOptions);
    }
}
