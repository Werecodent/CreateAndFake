using CreateAndFake.Design;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing Span collections for <see cref="IRandomizer"/>.</summary>
public sealed class SpanCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.SpanHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(Span<>), typeof(ReadOnlySpan<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

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
    private static object? Create(Type type, IRandomizerChainer randomizer)
    {
        Type content = type.GetGenericArguments().Single();
        Type arrayType = Array.CreateInstance(content, 0).GetType();

        return randomizer.Create(arrayType);
    }
}
