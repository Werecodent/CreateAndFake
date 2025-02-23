using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.CreateHints;

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
