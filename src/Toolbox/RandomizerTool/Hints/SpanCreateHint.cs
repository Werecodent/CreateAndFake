using CreateAndFake.Design;
using CreateAndFake.Design.Types;
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

        Type? asGeneric = GenericTypeConverter.AsGenericBase(type);
        if (asGeneric == typeof(Span<>) || asGeneric == typeof(ReadOnlySpan<>))
        {
            Type content = type.GetGenericArguments().Single();
            Type arrayType = Array.CreateInstance(content, 0).GetType();

            return new(randomizer.Create(arrayType));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
