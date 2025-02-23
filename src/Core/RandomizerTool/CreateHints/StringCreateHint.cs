using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="string"/> collections for <see cref="IRandomizer"/>.</summary>
public sealed class StringCreateHint : CreateHint<string>
{
    /// <inheritdoc/>
    protected override string Create(RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        char[] data = new char[randomizer.Options.NextStringSize()];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = randomizer.Options.Gen.NextItem(randomizer.Options.StringCharacterSet);
        }
        return new string(data);
    }
}
