global using MutatorMod = System.Func<
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions,
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool;

/// <summary>Changes the value of <see langword="object"/>s or creates alternatives.</summary>
public interface IMutator : IHintTool<MutatorOptions, IMutateHint>
{
    /// <summary>
    ///     Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.
    /// </summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IMutator WithOptions(MutatorMod optionConfiguration);

    /// <summary>
    ///     Creates a <typeparamref name="T"/> unequal by value to the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to differ from.</param>
    /// <inheritdoc cref="VariantOf{T}"/>
    T Variant<T>(T instance, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates an <see langword="object"/> unequal by value to the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to differ from.</param>
    /// <inheritdoc cref="VariantOf"/>
    object Variant(Type type, object? instance, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a <typeparamref name="T"/> unequal by value to the <paramref name="instances"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/>.</returns>
    /// <inheritdoc cref="VariantOf"/>
    T VariantOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates an <see langword="object"/> unequal by value to the <paramref name="instances"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of <see langword="object"/> to create.</param>
    /// <param name="instances">The <see langword="object"/>s to differ from.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>The created <see langword="object"/>.</returns>
    object VariantOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>
    ///     Creates a <typeparamref name="T"/> that shares
    ///     no values with the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to share no values with.</param>
    /// <inheritdoc cref="UniqueOf{T}"/>
    T Unique<T>(T instance, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates an <see langword="object"/> that shares
    ///     no values with the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to share no values with.</param>
    /// <inheritdoc cref="UniqueOf"/>
    object Unique(Type type, object? instance, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a <typeparamref name="T"/> that shares
    ///     no values with the <paramref name="instances"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/>.</returns>
    /// <inheritdoc cref="UniqueOf"/>
    T UniqueOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates an <see langword="object"/> that shares
    ///     no values with the <paramref name="instances"/>.
    /// </summary>
    /// <param name="instances">The <see langword="object"/>s to share no values with.</param>
    /// <remarks>
    ///     Ignores <see cref="Type"/>s with too small of range.
    ///     See <see cref="ExtractorOptions.UniqueIgnoredTypes"/>.
    /// </remarks>
    /// <inheritdoc cref="VariantOf"/>
    object UniqueOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>Attempts to mutate the <paramref name="instance"/>.</summary>
    /// <param name="instance">The <see langword="object"/> to try modifying.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="instance"/>
    ///     has been modified, <see langword="false"/> otherwise.
    /// </returns>
    bool Modify(object? instance, MutatorMod? optionConfiguration = null);
}
