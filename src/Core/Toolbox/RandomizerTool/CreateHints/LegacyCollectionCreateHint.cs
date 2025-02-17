using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.Specialized;
using CreateAndFake.Design;

#pragma warning disable IDE0058 // Expression value is never used: Return isn't present on all versions.

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing legacy collections for <see cref="IRandomizer"/>.</summary>
public sealed class LegacyCollectionCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ImmutableArray<(Type, Func<string[], RandomizerChainer, object>)> _Creators =
        [
            (typeof(Hashtable), CreateDict<Hashtable>),
            (typeof(SortedList), CreateDict<SortedList>),
            (typeof(ListDictionary), CreateDict<ListDictionary>),
            (typeof(HybridDictionary), CreateDict<HybridDictionary>),
            (typeof(StringDictionary), CreateDict<StringDictionary>),
            (typeof(OrderedDictionary), CreateDict<OrderedDictionary>),
            (typeof(NameValueCollection), CreateDict<NameValueCollection>),

            (typeof(Array), (data, gen) => data),
            (typeof(Stack), (data, gen) => new Stack(data)),
            (typeof(Queue), (data, gen) => new Queue(data)),
            (typeof(ArrayList), (data, gen) => new ArrayList(data)),
            (typeof(BitArray), (data, gen) => new BitArray(data.Select(d => gen.Create<bool>()).ToArray())),

            (typeof(StringCollection), (data, gen) =>
            {
                StringCollection result = [.. data];
                return result;
            }
            ),
        ];

    /// <summary>Collections that the hint will create.</summary>
    internal static IEnumerable<Type> PotentialCollections { get; } = _Creators.Select(i => i.Item1).ToFrozenSet();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer? randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type.Inherits<IEnumerable>() && FindMatches(type).Any())
        {
            int size = randomizer.Options.NextCollectionSize();

            return new(randomizer.Options.Gen.NextItem(FindMatches(type)).Item2
                .Invoke(CreateInternalData(size, randomizer), randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <summary>Finds potential collection matches for <paramref name="type"/>.</summary>
    /// <param name="type"><c>Type</c> to find matches for.</param>
    /// <returns>All possible matches.</returns>
    private static IEnumerable<(Type, Func<string[], RandomizerChainer, object>)> FindMatches(Type type)
    {
        return _Creators.Where(m => type.IsInheritedBy(m.Item1));
    }

    /// <summary>Creates the <typeparamref name="TDict"/> and populates it with data.</summary>
    /// <typeparam name="TDict"><c>Type</c> to create.</typeparam>
    /// <param name="keys">Keys to create in the <typeparamref name="TDict"/>.</param>
    /// <param name="gen">Handles randomizing child values.</param>
    /// <returns>The created instance.</returns>
    private static TDict CreateDict<TDict>(string[] keys, RandomizerChainer gen)
    {
        dynamic data = Activator.CreateInstance<TDict>()!;
        for (int i = 0; i < keys.Length; i++)
        {
            data.Add(keys[i], gen.Create<string>());
        }
        return data;
    }

    /// <summary>Creates populated collection of data to use.</summary>
    /// <param name="size">Number of items to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Data populated with random values.</returns>
    private static string[] CreateInternalData(int size, RandomizerChainer randomizer)
    {
        string[] data = new string[size];
        for (int i = 0; i < data.Length; i++)
        {
            data.SetValue(randomizer.Create<string>(), i);
        }
        return data;
    }
}

#pragma warning restore IDE0058 // Expression value is never used
