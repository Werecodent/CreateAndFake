using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing values for <see cref="IRandomizer"/>.</summary>
public sealed class ValueCreateHint : CreateHint
{
    /// <inheritdoc/>
    protected internal override (bool, object?) TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (randomizer.Options.Gen.Supports(type))
        {
            return (true, randomizer.Options.Gen.Next(type));
        }
        else
        {
            return (false, null);
        }
    }
}
