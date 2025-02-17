using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing immutable collections for <see cref="IRandomizer"/>.</summary>
public sealed class ImmutableCollectionCreateHint : CreateHint
{
    /// <summary>Collections able to be randomized.</summary>
    private static readonly ImmutableArray<(Type, MethodInfo)> _Collections =
    [
        (typeof(ImmutableList<>), FindCreateRangeBuilder(typeof(ImmutableList))),
        (typeof(ImmutableArray<>), FindCreateRangeBuilder(typeof(ImmutableArray))),
        (typeof(ImmutableQueue<>), FindCreateRangeBuilder(typeof(ImmutableQueue))),
        (typeof(ImmutableStack<>), FindCreateRangeBuilder(typeof(ImmutableStack))),
        (typeof(ImmutableHashSet<>), FindCreateRangeBuilder(typeof(ImmutableHashSet))),
        (typeof(ImmutableDictionary<,>), FindCreateRangeBuilder(typeof(ImmutableDictionary)))
    ];

    /// <summary>Finds the static <c>CreateRange</c> method for a collection.</summary>
    /// <param name="type">Collection type to create.</param>
    /// <returns>Found create method.</returns>
    private static MethodInfo FindCreateRangeBuilder(Type type)
    {
        return type
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == "CreateRange"
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType.Inherits(typeof(IEnumerable<>)));
    }

    /// <summary>Collections that the hint can create.</summary>
    internal static IEnumerable<Type> PotentialCollections { get; } = _Collections.Select(i => i.Item1).ToFrozenSet();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer? randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));
        if (type == null)
        {
            return CreateHintResult.None;
        }

        Type? itemType = GetItemType(type);
        if (itemType != null && FindMatches(type, itemType).Any())
        {
            return new(Create(type, itemType, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <param name="itemType">Item <c>Type</c> to be contained in the collection.</param>
    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object? Create(Type type, Type itemType, RandomizerChainer randomizer)
    {
        (Type, MethodInfo) chosen = randomizer.Options.Gen.NextItem(FindMatches(type, itemType));
        return chosen.Item2
            .MakeGenericMethod(type.GetGenericArguments())
            .Invoke(null, [randomizer.Create(typeof(IEnumerable<>).MakeGenericType(itemType), randomizer.Options)]);
    }

    /// <summary>Finds the <c>Type</c> to be contained by a created collection.</summary>
    /// <param name="type">Collection <c>Type</c> being created..</param>
    /// <returns><c>null</c> if not logical; <c>Type</c> for the collection otherwise.</returns>
    private static Type? GetItemType(Type type)
    {
        Type[] args = type.IsGenericType
            ? type.GetGenericArguments()
            : [];

        if (type.IsGenericTypeDefinition)
        {
            return null;
        }
        else if (args.Length == 1)
        {
            return args[0];
        }
        else if (args.Length == 2)
        {
            return typeof(KeyValuePair<,>).MakeGenericType(args[0], args[1]);
        }
        else
        {
            return null;
        }
    }

    /// <summary>Finds potential collection matches for <paramref name="type"/>.</summary>
    /// <param name="type"><c>Type</c> to find matches for.</param>
    /// <param name="itemType">Item <c>Type</c> to be contained in the collection.</param>
    /// <returns>All possible matches.</returns>
    private static IEnumerable<(Type, MethodInfo)> FindMatches(Type type, Type itemType)
    {
        Type typeAsGeneric = type.IsGenericType
            ? type.GetGenericTypeDefinition()
            : type;

        foreach ((Type, MethodInfo) match in _Collections.Where(c => typeAsGeneric.IsInheritedBy(c.Item1)))
        {
            if (!match.Item1.Inherits<IDictionary>() || itemType.Inherits(typeof(KeyValuePair<,>)))
            {
                yield return match;
            }
        }
    }
}
