using CreateAndFake.Design.Content;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Handles creation of a specific type.</summary>
internal interface ICreator : ITypeSupporter
{
    /// <summary>Creates a random instance of the specific type.</summary>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>The created instance.</returns>
    object? CreateSupported(IRandomizerChainer randomizer);
}
