using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing values for <see cref="IRandomizer"/>.</summary>
public sealed class ValueCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (randomizer.Options.Gen.Supports(type))
        {
            return new(randomizer.Options.Gen.Next(type));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
