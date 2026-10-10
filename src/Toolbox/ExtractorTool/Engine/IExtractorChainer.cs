using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Provides a callback into <see cref="IExtractor"/> to extract child values.</summary>
public interface IExtractorChainer : IExtractor, IToolChainer<ExtractorOptions, IExtractHint>
{
    /// <inheritdoc cref="AddFoundValueAsync"/>
    bool AddFoundValue(object value, ExtractorMod? optionConfiguration = null);

    /// <summary>Adds the <paramref name="value"/> as an extracted instance.</summary>
    /// <param name="value">Instance to add.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="value"/> was added, <see langword="false"/> otherwise.
    /// </returns>
    Task<bool> AddFoundValueAsync(
        object value,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    );

    /// <inheritdoc cref="InnerExtractAsync"/>
    bool InnerExtract(object? value, ExtractorMod? optionConfiguration = null);

    /// <summary>Extracts the members of the given <paramref name="value"/>.</summary>
    /// <param name="value">Instance to extract members from.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns><see langword="true"/> if data was extracted, <see langword="false"/> otherwise.</returns>
    Task<bool> InnerExtractAsync(
        object? value,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    );
}
