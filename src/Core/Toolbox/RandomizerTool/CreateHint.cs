namespace CreateAndFake.Toolbox.RandomizerTool;

/// <summary>Handles randomizing specific types for <see cref="IRandomizer"/>.</summary>
public abstract class CreateHint
{
    /// <summary>Tries to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type"><c>Type</c> to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Possible result.</returns>
    protected internal abstract CreateHintResult TryCreate(Type type, RandomizerChainer randomizer);
}
