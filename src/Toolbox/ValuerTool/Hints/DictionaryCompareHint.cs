using System.Collections;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IDictionary"/> collections for <see cref="IValuer"/>.</summary>
public sealed class DictionaryCompareHint : CompareHint<IDictionary>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.DictionaryHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer valuer
    )
    {
        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer valuer
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = actualKeys.FirstOrDefault(k => valuer.Equals(key, k));
            if (match != null)
            {
                foreach (Difference diff in valuer.Compare(expected[key], actual[match]))
                {
                    yield return new Difference($"[{TryDescribe(key)}]", diff);
                }
            }
            else
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference(expected[key], "'null'")
                );
            }
        }

        foreach (object key in actualKeys)
        {
            if (!expectedKeys.Any(k => valuer.Equals(key, k)))
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'null'", actual[key])
                );
            }
        }
    }

    /// <inheritdoc/>
    protected override async IAsyncEnumerable<Difference> CompareAsync(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = null;
            foreach (object potentialMatch in actualKeys)
            {
                if (await valuer.EqualsAsync(key, potentialMatch, canceler).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match != null)
            {
                await foreach (
                    Difference diff in valuer
                        .CompareAsync(expected[key], actual[match])
                        .WithCancellation(canceler)
                        .ConfigureAwait(false)
                )
                {
                    yield return new Difference($"[{TryDescribe(key)}]", diff);
                }
            }
            else
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference(expected[key], "'null'")
                );
            }
        }

        foreach (object key in actualKeys)
        {
            object? match = null;
            foreach (object potentialMatch in expectedKeys)
            {
                if (await valuer.EqualsAsync(key, potentialMatch, canceler).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match == null)
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'null'", actual[key])
                );
            }
        }
    }

    private static object TryDescribe(object item)
    {
        return (item is Type type) ? TypeDescriber.ExpandedName(type) : item;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IDictionary item, IValuerChainer valuer)
    {
        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += valuer.GetHashCode(entry);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        IDictionary item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += await valuer.GetHashCodeAsync(entry, canceler).ConfigureAwait(false);
        }
        return hash;
    }
}
