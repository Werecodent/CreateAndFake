using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Provides a callback into <see cref="IRandomizer"/> to create child values.</summary>
public interface IRandomizerChainer : IRandomizer, IToolChainer<RandomizerOptions, CreateHint>
{
    /// <summary>Checks if <typeparamref name="T"/> has already been created by the randomizer.</summary>
    /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
    /// <returns><see langword="true"/> if <typeparamref name="T"/> already created, <see langword="false"/> otherwise.</returns>
    bool AlreadyCreated<T>();

    /// <summary>Checks if <paramref name="type"/> has already been created by the randomizer.</summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> already created, <see langword="false"/> otherwise.</returns>
    bool AlreadyCreated(Type type);

    /// <summary>Calls the randomizer to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type">Type to create.</param>
    /// <param name="parent">Container of the instance to create.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>The created instance.</returns>
    object Create(Type type, object? parent, RandomizerMod? optionConfiguration = null);
}
