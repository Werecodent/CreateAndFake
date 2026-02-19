using System.Collections.Frozen;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing frozen collections for <see cref="IRandomizer"/>.</summary>
public sealed class FrozenCollectionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.FrozenCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(FrozenSet<>), typeof(FrozenDictionary<,>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = TypeDescriber.AsGenericBase(type);
        if (asGeneric == typeof(FrozenSet<>))
        {
            return new(
                FrozenSet.ToFrozenSet(
                    (dynamic)
                        randomizer.Create(
                            typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments()),
                            _ => randomizer.Options
                        )
                )
            );
        }
        else if (asGeneric == typeof(FrozenDictionary<,>))
        {
            Type itemType = typeof(KeyValuePair<,>).MakeGenericType(type.GetGenericArguments());
            return new(
                FrozenDictionary.ToFrozenDictionary(
                    (dynamic)
                        randomizer.Create(
                            typeof(IEnumerable<>).MakeGenericType(itemType),
                            _ => randomizer.Options
                        )
                )
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
