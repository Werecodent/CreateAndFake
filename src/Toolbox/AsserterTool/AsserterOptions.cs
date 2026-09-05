using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.AsserterTool;

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
    public StringComparison StringCompareOption { get; init; } = StringComparison.Ordinal;

    /// <summary>
    ///     If set to <see langword="true"/>, all exceptions will be rethrown instead of
    ///     successfully caught. Can be used to look at the exceptions being thrown during testing.
    /// </summary>
    [ConfigurableOption]
    public bool DisableAssertThrowCatching { get; init; } = false;

    /// <summary>Fails any call that utilizes the Debug methods.</summary>
    [ConfigurableOption]
    public bool DebugAssertsFail { get; init; } = false;

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
            DisableAssertThrowCatching = Config.GetValue(section, DisableAssertThrowCatching),
            StringCompareOption = Config.GetValue(section, StringCompareOption),
            DebugAssertsFail = Config.GetValue(section, DebugAssertsFail),
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(AsserterOptions);
    }
}
