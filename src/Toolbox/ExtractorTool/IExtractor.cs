global using ExtractorMod = System.Func<
    CreateAndFake.ExtractorTool.ExtractorOptions,
    CreateAndFake.ExtractorTool.ExtractorOptions
>;

namespace CreateAndFake.ExtractorTool;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractor
{
    /// <summary>Configured options for <c>this</c>.</summary>
    ExtractorOptions Options { get; }

    /// <summary>Finds data associated with <paramref name="source"/>.</summary>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns>Extracted content of <paramref name="source"/>.</returns>
    IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null);
}
