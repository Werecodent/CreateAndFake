using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.MutatorTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Fluent.Tooling;

/// <summary>Provides fluent randomization options.</summary>
/// <typeparam name="T"></typeparam>
/// <param name="source"><inheritdoc cref="Source" path="/summary"/></param>
/// <param name="tools"><inheritdoc cref="Tools" path="/summary"/></param>
public class ObjectTools<T>(T source, ToolSet? tools)
{
    /// <summary>Origin.</summary>
    protected T Source { get; } = source;

    /// <summary>Tools to use.</summary>
    protected ToolSet Tools { get; } = tools ?? CreateAndFake.Tools.Source;

    /// <summary>Accesses the raw fake wrapper.</summary>
    /// <returns>Fake to test with.</returns>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    public Fake<T> ToFake()
    {
        return new((IFaked)Source!);
    }

    /// <summary>Accesses the raw fake wrapper.</summary>
    /// <typeparam name="TNew"></typeparam>
    /// <returns>Fake to test with.</returns>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    public Fake<TNew> ToFake<TNew>()
    {
        return new((IFaked)Source!);
    }

    /// <inheritdoc cref="IDuplicator.Copy{T}(T,DuplicatorMod)"/>
    public T Copy(DuplicatorMod? optionConfiguration = null)
    {
        return Tools.Duplicator.Copy(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IExtractor.Extract(object,ExtractorMod)"/>
    public IContentMap Extract(ExtractorMod? optionConfiguration = null)
    {
        return Tools.Extractor.Extract(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.Variant{T}"/>
    public T Variant(MutatorMod? optionConfiguration = null)
    {
        return Tools.Mutator.Variant(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.Unique{T}"/>
    public T Unique(MutatorMod? optionConfiguration = null)
    {
        return Tools.Mutator.Unique(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.Modify"/>
    public bool Modify(MutatorMod? optionConfiguration = null)
    {
        return Tools.Mutator.Modify(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IRunner.CallMethodsOnAsync"/>
    public Task<RunResults> CallMethodsAsync(
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    )
    {
        return Tools.Runner.CallMethodsOnAsync(Source!, canceler, optionConfiguration);
    }

    /// <inheritdoc cref="IValuer.Equals(object,object)"/>
    public override bool Equals(object? obj)
    {
        return Tools.Valuer.Equals(Source, obj);
    }

    /// <inheritdoc cref="IValuer.Equals(object,object,ValuerMod)"/>
    public bool Equals(object? y, ValuerMod? optionConfiguration)
    {
        return Tools.Valuer.Equals(Source, y, optionConfiguration);
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

    /// <inheritdoc cref="IValuer.GetHashCode(object)"/>
    public override int GetHashCode()
    {
        return Tools.Valuer.GetHashCode(Source);
    }

    /// <inheritdoc cref="IValuer.GetHashCode(object,ValuerMod)"/>
    public int GetHashCode(ValuerMod? optionConfiguration)
    {
        return Tools.Valuer.GetHashCode(Source, optionConfiguration);
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
