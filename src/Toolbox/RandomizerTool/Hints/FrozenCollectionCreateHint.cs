using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing frozen collections for <see cref="IRandomizer"/>.</summary>
public sealed class FrozenCollectionCreateHint : CreateHint
{
    /// <summary>Constructs frozen sets.</summary>
    private static readonly MethodInfo _SetMaker = typeof(FrozenSet).GetMethod(
        nameof(FrozenSet.ToFrozenSet),
        BindingFlags.Public | BindingFlags.Static
    )!;

    /// <summary>Constructs frozen dictionaries.</summary>
    private static readonly MethodInfo _DictionaryMaker = typeof(FrozenDictionary)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name == nameof(FrozenDictionary.ToFrozenDictionary))
        .Single(m => m.GetParameters().Length == 2);

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        Type? asGeneric = type.AsGenericBase();

        if (asGeneric == typeof(FrozenSet<>))
        {
            return new(
                _SetMaker
                    .MakeGenericMethod(type.GetGenericArguments())
                    .Invoke(
                        null,
                        [
                            randomizer.Create(
                                typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments())
                            ),
                            null,
                        ]
                    )
            );
        }
        else if (asGeneric == typeof(FrozenDictionary<,>))
        {
            Type itemType = typeof(KeyValuePair<,>).MakeGenericType(type.GetGenericArguments());

            return new(
                _DictionaryMaker
                    .MakeGenericMethod(type.GetGenericArguments())
                    .Invoke(
                        null,
                        [
                            randomizer.Create(
                                typeof(IEnumerable<>).MakeGenericType(itemType),
                                randomizer.Options
                            ),
                            null,
                        ]
                    )
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
