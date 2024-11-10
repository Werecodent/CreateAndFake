using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent randomization options.</summary>
public static class CreateExtensions
{
    /// <inheritdoc cref="IDuplicator.Copy{T}(T,Func{DuplicatorOptions,DuplicatorOptions})"/>
    [return: NotNullIfNotNull(nameof(source))]
    public static T CreateDeepClone<T>(this T source)
    {
        return Tools.Duplicator.Copy(source);
    }

    /// <inheritdoc cref="IMutator.Variant{T}(T,T[])"/>
    public static T CreateVariant<T>(this T source)
    {
        return Tools.Mutator.Variant(source);
    }

    /// <inheritdoc cref="IMutator.Unique{T}(T,T[])"/>
    public static T CreateUnique<T>(this T source)
    {
        return Tools.Mutator.Unique(source);
    }

    /// <inheritdoc cref="IRandomizer.Create(Type,Func{RandomizerOptions,RandomizerOptions})"/>
    public static object CreateRandomInstance(this Type type)
    {
        return Tools.Randomizer.Create(type);
    }
}
