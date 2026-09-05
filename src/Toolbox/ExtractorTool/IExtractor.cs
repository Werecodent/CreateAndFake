global using ExtractorMod = System.Func<
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions,
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractor : IHintTool<ExtractorOptions, IExtractHint>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IExtractor WithOptions(ExtractorMod optionConfiguration);

    /// <inheritdoc cref="ExtractAsync"/>
    IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null);

    /// <summary>Finds data associated with <paramref name="source"/>.</summary>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>Extracted content of <paramref name="source"/>.</returns>
    Task<IAsyncContentMap> ExtractAsync(
        object? source,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    );
}
