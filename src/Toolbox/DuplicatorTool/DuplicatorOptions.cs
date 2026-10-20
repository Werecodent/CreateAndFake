using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.DuplicatorTool;

/// <summary>Settings for manipulating <see cref="IDuplicator"/> behavior.</summary>
public sealed record DuplicatorOptions : ToolHintOptions<DuplicatorOptions, ICopyHint>
{
    /// <summary>Identifier for the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Duplicator);

    /// <inheritdoc cref="IAsserter"/>
    public required IAsserter Asserter { get; init; }

    /// <inheritdoc cref="IExtractor"/>
    public required IExtractor Extractor { get; init; }

    /// <summary>If generated clones are compared to their original source before being returned.</summary>
    /// <remarks>
    ///     Useful for ensuring cloning is successfully accomplished by the <see cref="IDuplicator"/>.
    ///     <see cref="ValuerOptions.SkipAsyncValues"/> is overridden to <see langword="true"/>
    ///     for this verification in order to prevent <see cref="AsynchronousAccessException"/>s.
    /// </remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool VerifyEqualityForClones { get; init; } = true;

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal DuplicatorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            VerifyEqualityForClones = Config.GetValue(section, VerifyEqualityForClones),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
        };
    }
}
