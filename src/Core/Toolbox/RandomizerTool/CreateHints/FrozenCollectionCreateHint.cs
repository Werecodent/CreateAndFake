using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing frozen collections for <see cref="IRandomizer"/>.</summary>
public sealed class FrozenCollectionCreateHint : CreateHint
{
    /// <summary>Constructs frozen sets.</summary>
    private static readonly MethodInfo _SetMaker = typeof(FrozenSet)
        .GetMethod(nameof(FrozenSet.ToFrozenSet), BindingFlags.Public | BindingFlags.Static)!;

    /// <summary>Constructs frozen dictionaries.</summary>
    private static readonly MethodInfo _DictionaryMaker = typeof(FrozenDictionary)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(m => m.Name == nameof(FrozenDictionary.ToFrozenDictionary) && m.GetParameters().Length == 2);

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        Type? asGeneric = type.AsGenericType();

        if (asGeneric == typeof(FrozenSet<>))
        {
            return new(_SetMaker
                .MakeGenericMethod(type.GetGenericArguments())
                .Invoke(null, [randomizer.Create(typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments())), null]));
        }
        else if (asGeneric == typeof(FrozenDictionary<,>))
        {
            Type itemType = typeof(KeyValuePair<,>).MakeGenericType(type.GetGenericArguments());

            return new(_DictionaryMaker
                .MakeGenericMethod(type.GetGenericArguments())
                .Invoke(null, [randomizer.Create(typeof(IEnumerable<>).MakeGenericType(itemType), randomizer.Options), null]));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}