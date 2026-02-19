using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing collections for <see cref="IRandomizer"/>.</summary>
public sealed class CollectionCreateHint : CreateHint
{
    /// <summary>Collections able to be randomized.</summary>
    private static readonly ImmutableArray<Type> _Collections =
    [
        typeof(List<>),
        typeof(Queue<>),
        typeof(Stack<>),
        typeof(HashSet<>),
        typeof(LinkedList<>),
        typeof(Dictionary<,>),
        typeof(ConcurrentQueue<>),
        typeof(ConcurrentStack<>),
        typeof(ConcurrentDictionary<,>),
    ];

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.CollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => PotentialCollections;

    /// <summary>Collections that the hint can create.</summary>
    internal static IEnumerable<Type> PotentialCollections { get; } =
        _Collections.Select(i => i).ToFrozenSet();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer? randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);
        if (type == null)
        {
            return CreateHintResult.None;
        }

        Type? itemType = TypeDescriber
            .AsConcreteType(type, typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];

        if (itemType != null && FindMatches(type, itemType).Any())
        {
            return randomizer.Options.CollectionAttempts.Retry(
                $"Generating '{TypeDescriber.ExpandedName(type)}' collection.",
                () => new CreateHintResult(Create(type, itemType, randomizer))
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <param name="itemType">Item <see cref="Type"/> to be contained in the collection.</param>
    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object? Create(Type type, Type itemType, IRandomizerChainer randomizer)
    {
        Array internalData = CreateInternalData(itemType, randomizer);

        Type collection = randomizer.Options.Gen.NextItem(FindMatches(type, itemType));
        Type newType = MakeNewType(collection, itemType);

        if (newType == typeof(Array) || newType == internalData.GetType())
        {
            return internalData;
        }
#if LEGACY // Constructor missing in .NET 4.8.
        else if (collection == typeof(Dictionary<,>))
        {
            dynamic result = Activator.CreateInstance(newType);
            foreach (dynamic item in internalData)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }
#endif
        else
        {
            return Activator.CreateInstance(newType, internalData);
        }
    }

    /// <summary>Finds potential collection matches for <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to find matches for.</param>
    /// <param name="itemType">Item <see cref="Type"/> to be contained in the collection.</param>
    /// <returns>All possible matches.</returns>
    private static IEnumerable<Type> FindMatches(Type type, Type itemType)
    {
        Type typeAsGeneric = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

        if (
            type.IsArray
            || type.IsInheritedBy<IList>()
            || typeAsGeneric.IsInheritedBy(typeof(IList<>))
        )
        {
            yield return typeof(Array);
        }

        foreach (Type match in PotentialCollections.Where(typeAsGeneric.IsInheritedBy))
        {
            if (!match.Inherits<IDictionary>() || itemType.Inherits(typeof(KeyValuePair<,>)))
            {
                yield return match;
            }
        }
    }

    /// <summary>Creates basic structures for <paramref name="itemType"/>.</summary>
    /// <param name="itemType">Item <see cref="Type"/> to be contained in the collection.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Data populated with random values.</returns>
    private static Array CreateInternalData(Type itemType, IRandomizerChainer randomizer)
    {
        Array data = Array.CreateInstance(itemType, randomizer.Options.NextCollectionSize());
        for (int i = 0; i < data.Length; i++)
        {
            data.SetValue(randomizer.Create(itemType), i);
        }
        return data;
    }

    /// <summary>Constructs the new <see cref="Type"/> to make for the collection.</summary>
    /// <param name="collection">Matching collection <see cref="Type"/> with details.</param>
    /// <param name="itemType">Item <see cref="Type"/> to be contained in the collection.</param>
    /// <returns><see cref="Type"/> to create.</returns>
    private static Type MakeNewType(Type collection, Type itemType)
    {
        if (!collection.IsGenericType)
        {
            return collection;
        }
        else if (collection.Inherits<IDictionary>())
        {
            return collection.MakeGenericType(itemType.GetGenericArguments());
        }
        else
        {
            return collection.MakeGenericType(itemType);
        }
    }
}
