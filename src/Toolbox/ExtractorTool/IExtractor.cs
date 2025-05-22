global using ExtractorMod = System.Func<
    CreateAndFake.ExtractorTool.ExtractorOptions,
    CreateAndFake.ExtractorTool.ExtractorOptions
>;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractor : ITool<ExtractorOptions>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    IExtractor WithOptions(ExtractorMod optionConfiguration);

    /// <summary>Finds data associated with <paramref name="source"/>.</summary>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>Extracted content of <paramref name="source"/>.</returns>
    IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null);
}
