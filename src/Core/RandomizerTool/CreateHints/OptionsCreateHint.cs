using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="RandomizerOptions"/> for <see cref="IRandomizer"/>.</summary>
public sealed class OptionsCreateHint : CreateHint<RandomizerOptions>
{
    /// <inheritdoc/>
    protected override RandomizerOptions Create(RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        return randomizer.Options with
        {
            Gen = randomizer.Create<SeededRandom>(),
            CollectionMinSize = randomizer.Options.Gen.Next(0, 2),
            CollectionMaxSize = randomizer.Options.Gen.Next(0, 5),
            StringMinSize = randomizer.Options.Gen.Next(0, 4),
            StringMaxSize = randomizer.Options.Gen.Next(0, 10)
        };
    }
}
