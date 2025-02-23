using System.Diagnostics.CodeAnalysis;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent randomization options.</summary>
public static class CreateExtensions
{
    /// <inheritdoc cref="IDuplicator.Copy{T}(T,DuplicatorMod)"/>
    [return: NotNullIfNotNull(nameof(source))]
    public static T CreateDeepClone<T>(this T source, DuplicatorMod? optionConfiguration = null)
    {
        return Tools.Duplicator.Copy(source, optionConfiguration);
    }

    /// <inheritdoc cref="IMutator.Variant{T}"/>
    public static T CreateVariant<T>(this T source)
    {
        return Tools.Mutator.Variant(source);
    }

    /// <inheritdoc cref="IMutator.Unique{T}"/>
    public static T CreateUnique<T>(this T source)
    {
        return Tools.Mutator.Unique(source);
    }

    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    public static object CreateRandomInstance(this Type type, RandomizerMod? optionConfiguration = null)
    {
        return Tools.Randomizer.Create(type, optionConfiguration);
    }
}
