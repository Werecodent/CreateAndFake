using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool;

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
        try
        {
            return new ExtractorChainer(Options, _Engine).Extract(source, optionConfiguration);
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
    public async Task<IAsyncContentMap> ExtractAsync(
        object? source,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    )
    {
        try
        {
            return await new ExtractorChainer(Options, _Engine)
                .ExtractAsync(source, canceler, optionConfiguration)
                .ConfigureAwait(false);
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
