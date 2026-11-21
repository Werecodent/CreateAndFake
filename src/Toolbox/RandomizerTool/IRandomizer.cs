global using RandomizerMod = System.Func<
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions,
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool;

/// <summary>Tool to create <see cref="object"/>s populated with random values.</summary>
/// <remarks>Useful for generating data to test with.</remarks>
public interface IRandomizer : IHintTool<RandomizerOptions, ICreateHint>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IRandomizer WithOptions(RandomizerMod optionConfiguration);

    /// <summary>Creates a randomized <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T"><see cref="Type"/> to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    /// <inheritdoc cref="Create(Type,RandomizerMod)"/>
    T Create<T>(RandomizerMod? optionConfiguration = null);

    /// <summary>Creates a randomized instance of the <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to create.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>The created instance.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RandomizerOptions.FinalCondition"/></item>
    ///         <item><see cref="RandomizerOptions.StringMinSize"/></item>
    ///         <item><see cref="RandomizerOptions.StringMaxSize"/></item>
    ///         <item><see cref="RandomizerOptions.CollectionMinSize"/></item>
    ///         <item><see cref="RandomizerOptions.CollectionMaxSize"/></item>
    ///         <item><see cref="RandomizerOptions.StringCharacterSet"/></item>
    ///         <item><see cref="RandomizerOptions.PreferLocalSubclasses"/></item>
    ///         <item><see cref="RandomizerOptions.CreateObjectAttemptLimit"/></item>
    ///         <item><see cref="RandomizerOptions.RequireRandomizedContents"/></item>
    ///         <item><see cref="RandomizerOptions.FinalConditionAttemptLimit"/></item>
    ///         <item><see cref="RandomizerOptions.CreateCollectionAttemptLimit"/></item>
    ///     </list>
    /// </remarks>
    object Create(Type type, RandomizerMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a randomized <typeparamref name="T"/> instance restricted to the given <paramref name="size"/>.
    /// </summary>
    /// <inheritdoc cref="CreateSized"/>
    /// <inheritdoc cref="Create{T}"/>
    T CreateSized<T>(int size, RandomizerMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a randomized instance of the <paramref name="type"/>
    ///     restricted to the given <paramref name="size"/>.
    /// </summary>
    /// <param name="size">Specific size to require of the top level collection.</param>
    /// <remarks>
    ///     The given <paramref name="size"/> does not effect the size of created nested collections.
    ///     <inheritdoc cref="Create" path="/remarks"/></remarks>
    /// <inheritdoc cref="Create"/>
    object CreateSized(int size, Type type, RandomizerMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a <typeparamref name="T"/> instance using <paramref name="values"/> or random data as needed.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    /// <inheritdoc cref="Inject"/>
    T Inject<T>(IEnumerable<object?>? values, RandomizerMod? optionConfiguration = null);

    /// <summary>Creates an instance using <paramref name="values"/> or random data as needed.</summary>
    /// <param name="type"><see cref="Type"/> to create.</param>
    /// <param name="values">Values to inject into the instance.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>The created instance.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RandomizerOptions.StringMinSize"/></item>
    ///         <item><see cref="RandomizerOptions.StringMaxSize"/></item>
    ///         <item><see cref="RandomizerOptions.CollectionMinSize"/></item>
    ///         <item><see cref="RandomizerOptions.CollectionMaxSize"/></item>
    ///         <item><see cref="RandomizerOptions.StringCharacterSet"/></item>
    ///         <item><see cref="RandomizerOptions.PreferLocalSubclasses"/></item>
    ///         <item><see cref="RandomizerOptions.CreateObjectAttemptLimit"/></item>
    ///         <item><see cref="RandomizerOptions.RequireRandomizedContents"/></item>
    ///         <item><see cref="RandomizerOptions.CreateCollectionAttemptLimit"/></item>
    ///     </list>
    /// </remarks>
    object Inject(
        Type type,
        IEnumerable<object?>? values,
        RandomizerMod? optionConfiguration = null
    );
}
