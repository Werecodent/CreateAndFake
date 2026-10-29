global using ExtractorMod = System.Func<
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions,
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Tool to extract the contents of <see cref="object"/>s.</summary>
/// <remarks>Useful for determining what data is shared among <see cref="object"/>s.</remarks>
public interface IExtractor : IHintTool<ExtractorOptions, IExtractHint>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IExtractor WithOptions(ExtractorMod optionConfiguration);

    /// <inheritdoc cref="ExtractAsync"/>
    IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null);

    /// <summary>Finds all data contained by the <paramref name="source"/> instance.</summary>
    /// <param name="source">Instance being deconstructed.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>A mapping for the extracted content of <paramref name="source"/>.</returns>
    /// <remarks>
    ///     The resulting map instance provides a means of searching all the content
    ///     or comparing with the extracted results of other <see cref="object"/>s.
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="ExtractorOptions.NonExtractableTypes"/></item>
    ///         <item><see cref="ExtractorOptions.ExtractPrivateMembers"/></item>
    ///         <item><see cref="ExtractorOptions.IgnoredTypesForUniques"/></item>
    ///     </list>
    /// </remarks>
    Task<IAsyncContentMap> ExtractAsync(
        object? source,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    );
}
