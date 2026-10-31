using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.RandomizerTool.Engine;

/// <summary>Handles randomizing specific types for <see cref="IRandomizer"/>.</summary>
public interface ICreateHint : IToolHint
{
    /// <summary>Tries to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Possible result.</returns>
    CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer);
}
