using System.Collections.Immutable;
using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool.Engine;
using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.ExtractorTool;

/// <inheritdoc cref="IExtractor"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Extractor(ExtractorOptions options) : IExtractor
{
    /// <summary>Default set of hints to use for copying.</summary>
    internal static readonly ImmutableArray<ExtractHint> DefaultHints =
    [
        new NullExtractHint(),
        new EndingExtractHint(),
        new DictionaryExtractHint(),
        new EnumerableExtractHint(),
        new DelegateExtractHint(),
        new TaskExtractHint(),
        new ObjectExtractHint(),
    ];

    /// <summary>Handles hint based extraction.</summary>
    private static readonly ExtractorEngine _engine = new(DefaultHints);

    /// <inheritdoc/>
    public ExtractorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

    /// <inheritdoc/>
    public IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null)
    {
        ExtractorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            return new ExtractorChainer(localOptions, _engine).Extract(source);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue extracting type '{source?.GetType().Name}'.", e);
        }
    }

    /// <inheritdoc/>
    public IExtractor WithOptions(ExtractorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Extractor(optionConfiguration.Invoke(Options));
    }
}
