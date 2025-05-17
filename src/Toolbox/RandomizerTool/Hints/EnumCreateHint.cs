using CreateAndFake.Design;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <c>Enum</c> values for <see cref="IRandomizer"/>.</summary>
public sealed class EnumCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type?.IsEnum ?? false)
        {
            return new(randomizer.Options.Gen.NextItem(Enum.GetValues(type).Cast<object>()));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
