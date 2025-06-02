global using MutatorMod = System.Func<
    CreateAndFake.MutatorTool.MutatorOptions,
    CreateAndFake.MutatorTool.MutatorOptions
>;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool;

/// <summary>Changes the value of objects or creates alternatives.</summary>
public interface IMutator : ITool<MutatorOptions>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> for the new tool.</param>
    /// <returns>The created tool.</returns>
    IMutator WithOptions(MutatorMod optionConfiguration);

    /// <typeparam name="T"><c>Type</c> to create.</typeparam>
    /// <inheritdoc cref="Variant"/>
    T Variant<T>(T instance, params IEnumerable<T?>? extraInstances);

    /// <summary>Creates an object with different values.</summary>
    /// <param name="type"><c>Type</c> to create.</param>
    /// <param name="instance">Object to diverge from.</param>
    /// <param name="extraInstances">Extra objects to diverge from.</param>
    /// <returns>
    ///     The created object that differs from <paramref name="instance"/> and <paramref name="extraInstances"/>.
    /// </returns>
    object Variant(Type type, object? instance, params IEnumerable<object?>? extraInstances);

    /// <typeparam name="T"><c>Type</c> to create.</typeparam>
    /// <inheritdoc cref="Unique"/>
    T Unique<T>(T instance, params IEnumerable<T?>? extraInstances);

    /// <summary>Creates an object with completely different values.</summary>
    /// <param name="type"><c>Type</c> to create.</param>
    /// <param name="instance">Object to diverge from.</param>
    /// <param name="extraInstances">Extra objects to diverge from.</param>
    /// <returns>
    ///     The created object that differs from <paramref name="instance"/> and <paramref name="extraInstances"/>.
    /// </returns>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    object Unique(Type type, object? instance, params IEnumerable<object?>? extraInstances);

    /// <summary>Attempts to mutate an object.</summary>
    /// <param name="instance">Object to modify.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns><see langword="true"/> if <paramref name="instance"/> has been modified; <see langword="false"/> otherwise.</returns>
    bool Modify(object? instance, MutatorMod? optionConfiguration = null);
}
