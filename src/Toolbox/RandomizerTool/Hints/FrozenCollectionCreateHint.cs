using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing frozen collections for <see cref="IRandomizer"/>.</summary>
public sealed class FrozenCollectionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.FrozenCollectionHint;

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
                _SetMaker
                    .MakeGenericMethod(type.GetGenericArguments())
                    .Invoke(
                        null,
                        [
                            randomizer.Create(
                                typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments()),
                                _ => randomizer.Options
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
                                _ => randomizer.Options
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
