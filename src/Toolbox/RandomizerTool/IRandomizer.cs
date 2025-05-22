global using RandomizerMod = System.Func<
    CreateAndFake.RandomizerTool.RandomizerOptions,
    CreateAndFake.RandomizerTool.RandomizerOptions
>;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool;

/// <summary>Creates objects and populates them with random values.</summary>
public interface IRandomizer : ITool<RandomizerOptions>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    IRandomizer WithOptions(RandomizerMod optionConfiguration);

    /// <summary>Creates a randomized <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Type to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    /// <inheritdoc cref="Create(Type,RandomizerMod)"/>
    T Create<T>(RandomizerMod? optionConfiguration = null);

    /// <summary>Creates a randomized instance.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>The created instance.</returns>
    /// <exception cref="NotSupportedException">If no hint supports generating the type.</exception>
    /// <exception cref="TimeoutException">If an instance couldn't be created to match the condition.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    object Create(Type type, RandomizerMod? optionConfiguration = null);

    /// <summary>
    ///     Creates a <typeparamref name="T"/> instance using <paramref name="values"/> or random data as needed.
    /// </summary>
    /// <typeparam name="T">Type to create.</typeparam>
    /// <param name="values">Values to inject into the <typeparamref name="T"/> instance.</param>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    T Inject<T>(params IEnumerable<object?>? values);

    /// <summary>Creates an instance using <paramref name="values"/> or random data as needed.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="values">Values to inject into the instance.</param>
    /// <returns>The created instance.</returns>
    object Inject(Type type, params IEnumerable<object?>? values);
}
