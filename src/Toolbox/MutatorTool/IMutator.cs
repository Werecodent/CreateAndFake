global using MutatorMod = System.Func<
    CreateAndFake.MutatorTool.MutatorOptions,
    CreateAndFake.MutatorTool.MutatorOptions>;
namespace CreateAndFake.MutatorTool;

/// <summary>Changes the value of objects or creates alternatives.</summary>
public interface IMutator
{
    /// <summary>Configured options for <c>this</c>.</summary>
    MutatorOptions Options { get; }

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
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns><c>true</c> if <paramref name="instance"/> has been modified; <c>false</c> otherwise.</returns>
    bool Modify(object? instance, MutatorMod? optionConfiguration = null);
}
