global using MutatorMod = System.Func<
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions,
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool;

/// <summary>Tool to modify the state of <see langword="object"/>s or create variants.</summary>
/// <remarks>Useful for testing behavior under various states.</remarks>
public interface IMutator : IHintTool<MutatorOptions, IMutateHint>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IMutator WithOptions(MutatorMod optionConfiguration);

    /// <summary>Creates a <typeparamref name="T"/> unequal by value with the <paramref name="instance"/>.</summary>
    /// <param name="instance">The <typeparamref name="T"/> to differentiate from.</param>
    /// <inheritdoc cref="VariantOf{T}"/>
    T Variant<T>(T instance, MutatorMod? optionConfiguration = null);

    /// <summary>Creates an <see langword="object"/> unequal by value with the <paramref name="instance"/>.</summary>
    /// <param name="instance">The <see langword="object"/> to differentiate from.</param>
    /// <inheritdoc cref="VariantOf"/>
    object Variant(Type type, object? instance, MutatorMod? optionConfiguration = null);

    /// <inheritdoc cref="Variant{T}"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> VariantAsync<T>(
        T instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <inheritdoc cref="Variant"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> VariantAsync(
        Type type,
        object? instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <inheritdoc cref="VariantOfAsync{T}"/>
    T VariantOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null);

    /// <inheritdoc cref="VariantOfAsync"/>
    object VariantOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>Creates a <typeparamref name="T"/> unequal by value with the <paramref name="instances"/>.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to create.</typeparam>
    /// <returns>The created unequal <typeparamref name="T"/>.</returns>
    /// <inheritdoc cref="VariantOfAsync"/>
    Task<T> VariantOfAsync<T>(
        IEnumerable<T?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>Creates an <see langword="object"/> unequal by value with the <paramref name="instances"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to create.</param>
    /// <param name="instances">The <see langword="object"/>s to differ from.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>The created unequal <see langword="object"/>.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="MutatorOptions.AttemptLimitForVariants"/></item>
    ///     </list>
    /// </remarks>
    Task<object> VariantOfAsync(
        Type type,
        IEnumerable<object?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>
    ///     Creates a <typeparamref name="T"/> that shares no values with the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to share no values with.</param>
    /// <inheritdoc cref="UniqueOf{T}"/>
    T Unique<T>(T instance, MutatorMod? optionConfiguration = null);

    /// <summary>
    ///     Creates an <see langword="object"/> that shares no values with the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The <see langword="object"/> to share no values with.</param>
    /// <inheritdoc cref="UniqueOf"/>
    object Unique(Type type, object? instance, MutatorMod? optionConfiguration = null);

    /// <inheritdoc cref="Unique{T}"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> UniqueAsync<T>(
        T instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <inheritdoc cref="Unique"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> UniqueAsync(
        Type type,
        object? instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <inheritdoc cref="UniqueOfAsync{T}"/>
    T UniqueOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null);

    /// <inheritdoc cref="UniqueOfAsync"/>
    object UniqueOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>
    ///     Creates a <typeparamref name="T"/> that shares no values with the <paramref name="instances"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/>.</returns>
    /// <inheritdoc cref="UniqueOfAsync"/>
    Task<T> UniqueOfAsync<T>(
        IEnumerable<T?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>
    ///     Creates an <see langword="object"/> that shares no values with the <paramref name="instances"/>.
    /// </summary>
    /// <param name="instances">The <see langword="object"/>s to share no values with.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="MutatorOptions.AttemptLimitForUniques"/></item>
    ///         <item><see cref="ExtractorOptions.NonExtractableTypes"/></item>
    ///         <item><see cref="ExtractorOptions.ExtractPrivateMembers"/></item>
    ///         <item><see cref="ExtractorOptions.IgnoredTypesForUniques"/></item>
    ///     </list>
    /// </remarks>
    /// <inheritdoc cref="VariantOfAsync"/>
    Task<object> UniqueOfAsync(
        Type type,
        IEnumerable<object?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    );

    /// <summary>Attempts to mutate the <paramref name="instance"/>.</summary>
    /// <param name="instance">The <see langword="object"/> to try modifying.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="instance"/>
    ///     has been modified, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="MutatorOptions.MaxAttemptsForMutatingSets"/></item>
    ///     </list>
    /// </remarks>
    bool Modify(object? instance, MutatorMod? optionConfiguration = null);
}
