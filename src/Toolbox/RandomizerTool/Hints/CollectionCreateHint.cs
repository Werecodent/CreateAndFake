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
    private static readonly FrozenDictionary<Type, Func<Type, Array, object?>> _Collections =
        new Dictionary<Type, Func<Type, Array, object?>>()
        {
            { typeof(List<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(Queue<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(Stack<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(HashSet<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(LinkedList<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(ConcurrentQueue<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(ConcurrentStack<>), (type, data) => Activator.CreateInstance(type, data) },
            { typeof(FrozenSet<>), (_, data) => FrozenSet.ToFrozenSet((dynamic)data) },
            { typeof(ImmutableList<>), (_, data) => ImmutableList.CreateRange((dynamic)data) },
            { typeof(ImmutableArray<>), (_, data) => ImmutableArray.CreateRange((dynamic)data) },
            { typeof(ImmutableQueue<>), (_, data) => ImmutableQueue.CreateRange((dynamic)data) },
            { typeof(ImmutableStack<>), (_, data) => ImmutableStack.CreateRange((dynamic)data) },
            {
                typeof(ConcurrentDictionary<,>),
                (type, data) => Activator.CreateInstance(type, data)
            },
            {
                typeof(FrozenDictionary<,>),
                (_, data) => FrozenDictionary.ToFrozenDictionary((dynamic)data)
            },
            {
                typeof(ImmutableHashSet<>),
                (_, data) => ImmutableHashSet.CreateRange((dynamic)data)
            },
            {
                typeof(ImmutableDictionary<,>),
                (_, data) => ImmutableDictionary.CreateRange((dynamic)data)
            },
            {
                typeof(Dictionary<,>),
                (type, data) =>
                {
#if LEGACY // Constructor missing in .NET 4.8.
                    dynamic result = Activator.CreateInstance(type);
                    foreach (dynamic item in data)
                    {
                        result.Add(item.Key, item.Value);
                    }
                    return result;
#else
                    return Activator.CreateInstance(type, data);
#endif
                }
            },
        }.ToFrozenDictionary();

    /// <summary>Collections that the hint can create.</summary>
    internal static IEnumerable<Type> PotentialCollections { get; } =
        _Collections
            .Keys.SelectMany(t => InheritanceTracker.For(t).InheritedTypes)
            .Where(t => t.Inherits(typeof(IEnumerable<>)))
            .Select(t => TypeDescriber.AsGenericBase(t) ?? t)
            .Distinct()
            .ToFrozenSet()!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.CollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => PotentialCollections;

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
        else
        {
            return _Collections[collection].Invoke(newType, internalData);
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
