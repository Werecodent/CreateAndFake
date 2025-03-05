using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.CreateHints;

/// <summary>Handles randomizing Span collections for <see cref="IRandomizer"/>.</summary>
public sealed class SpanCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type.Inherits(typeof(Span<>)) || type.Inherits(typeof(ReadOnlySpan<>)))
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object? Create(Type type, RandomizerChainer randomizer)
    {
        Type content = type.GetGenericArguments().Single();
        Type arrayType = Array.CreateInstance(content, 0).GetType();

        return randomizer.Create(arrayType, randomizer.Parent);
    }
}
