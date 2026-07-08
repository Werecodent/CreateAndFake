using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool;

/// <inheritdoc cref="IExtractor"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Extractor(ExtractorOptions options) : IExtractor
{
    /// <summary>Handles hint based extraction.</summary>
    private static readonly ExtractorEngine _Engine = new();

    /// <inheritdoc/>
    public ExtractorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _Engine.SupportedTypes;

    /// <inheritdoc/>
    public IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null)
    {
        ExtractorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            return new ExtractorChainer(localOptions, _Engine).Extract(source);
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Issue extracting type '{GenericConverter.ExpandName(source)}'.",
                e
            );
        }
    }

    /// <inheritdoc/>
    public IExtractor WithOptions(ExtractorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Extractor(optionConfiguration.Invoke(Options));
    }
}
