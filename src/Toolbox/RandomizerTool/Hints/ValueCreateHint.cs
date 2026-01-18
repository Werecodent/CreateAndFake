using CreateAndFake.Design;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing values for <see cref="IRandomizer"/>.</summary>
public sealed class ValueCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

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
