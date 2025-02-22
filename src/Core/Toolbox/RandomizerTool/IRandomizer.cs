global using RandomizerMod = System.Func<
    CreateAndFake.Toolbox.RandomizerTool.RandomizerOptions,
    CreateAndFake.Toolbox.RandomizerTool.RandomizerOptions>;
using System.Reflection;

namespace CreateAndFake.Toolbox.RandomizerTool;

/// <summary>Creates objects and populates them with random values.</summary>
public interface IRandomizer
{
    /// <summary>Configured options for <c>this</c>.</summary>
    RandomizerOptions Options { get; }

    /// <summary>Creates a randomized <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Type to create.</typeparam>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    /// <inheritdoc cref="Create(Type,RandomizerMod)"/>
    T Create<T>(RandomizerMod? optionConfiguration = null);

    /// <summary>Creates a randomized instance.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns>The created instance.</returns>
    /// <exception cref="NotSupportedException">If no hint supports generating the type.</exception>
    /// <exception cref="TimeoutException">If an instance couldn't be created to match the condition.</exception>
    /// <exception cref="InsufficientExecutionStackException">If infinite recursion occurs.</exception>
    object Create(Type type, RandomizerMod? optionConfiguration = null);

    /// <summary>
    ///     Constructs the parameters for <paramref name="method"/>.
    ///     Randomizes types by default.
    ///     Earlier types will be used to construct later types if possible.
    /// </summary>
    /// <param name="method">Method to create parameters for.</param>
    /// <param name="values">Starting values to inject into instances.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    MethodCallWrapper CreateFor(MethodBase method, params IEnumerable<object?>? values);

    /// <summary>
    ///     Constructs the parameters for <paramref name="method"/>.
    ///     Randomizes types by default.
    ///     Earlier types will be used to construct later types if possible.
    /// </summary>
    /// <param name="method">Method to create parameters for.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <param name="values">Starting values to inject into instances.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    MethodCallWrapper CreateFor(MethodBase method,
        RandomizerMod optionConfiguration, params IEnumerable<object?>? values);

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
