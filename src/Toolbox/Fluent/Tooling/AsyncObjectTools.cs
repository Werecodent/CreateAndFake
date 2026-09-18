using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Fluent.Tooling;

/// <inheritdoc/>
public class AsyncObjectTools<T>(T source, ToolSet? tools) : ObjectTools<T>(source, tools)
{
    /// <inheritdoc cref="IExtractor.Extract(object,ExtractorMod)"/>
    public Task<IAsyncContentMap> ExtractAsync(
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    )
    {
        return Tools.Extractor.ExtractAsync(Source, canceler, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.Variant{T}"/>
    public Task<T> VariantAsync(CancellationToken canceler, MutatorMod? optionConfiguration = null)
    {
        return Tools.Mutator.VariantAsync(Source, canceler, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.UniqueAsync{T}"/>
    public Task<T> UniqueAsync(CancellationToken canceler, MutatorMod? optionConfiguration = null)
    {
        return Tools.Mutator.UniqueAsync(Source, canceler, optionConfiguration);
    }

    /// <inheritdoc cref="IValuer.EqualsAsync(object,object,CancellationToken,ValuerMod)"/>
    public Task<bool> EqualsAsync(
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return Tools.Valuer.EqualsAsync(Source, y, canceler, optionConfiguration);
    }

    /// <inheritdoc cref="IValuer.GetHashCodeAsync(object,CancellationToken,ValuerMod)"/>
    public Task<int> GetHashCodeAsync(
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return Tools.Valuer.GetHashCodeAsync(Source, canceler, optionConfiguration);
    }
}
