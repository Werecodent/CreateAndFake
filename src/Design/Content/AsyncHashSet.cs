using System.Runtime.CompilerServices;
using System.Text;
using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.Design.Content;

/// <inheritdoc cref="IAsyncSet{T}"/>
public sealed class AsyncHashSet<T> : IAsyncSet<T>
{
    /// <summary>Determines value equality for the set.</summary>
    private readonly IAsyncEqualityComparer<T> _comparer;

    /// <summary>Current contents for the set.</summary>
    private readonly Task<Dictionary<int, List<T>>> _contents;

    /// <summary>Creates an empty set.</summary>
    /// <param name="comparer"><inheritdoc cref="_comparer" path="/summary"/></param>
    public AsyncHashSet(IAsyncEqualityComparer<T> comparer)
        : this(Task.FromResult(new Dictionary<int, List<T>>()), comparer) { }

    /// <summary>Creates a populated set.</summary>
    /// <param name="contents"><inheritdoc cref="_contents" path="/summary"/></param>
    /// <param name="comparer"><inheritdoc cref="_comparer" path="/summary"/></param>
    private AsyncHashSet(
        Task<Dictionary<int, List<T>>> contents,
        IAsyncEqualityComparer<T> comparer
    )
    {
        ArgumentGuard.ThrowIfNull(comparer);

        _comparer = comparer;
        _contents = contents;
    }

    /// <summary>Creates a set with initial <paramref name="contents"/>.</summary>
    /// <param name="contents"><inheritdoc cref="_contents" path="/summary"/></param>
    /// <param name="comparer"><inheritdoc cref="_comparer" path="/summary"/></param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The created set.</returns>
    public static AsyncHashSet<T> CreateFromAsync(
        IEnumerable<T> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        return CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(contents, iterationLimit, canceler),
            comparer,
            iterationLimit,
            canceler
        );
    }

    /// <inheritdoc cref="CreateFromAsync(IEnumerable{T},IAsyncEqualityComparer{T},int,CancellationToken)"/>
    public static AsyncHashSet<T> CreateFromAsync(
        IAsyncEnumerable<T> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        return new AsyncHashSet<T>(
            SetInitialContentsAsync(contents, comparer, iterationLimit, canceler),
            comparer
        );
    }

    private static async Task<Dictionary<int, List<T>>> SetInitialContentsAsync(
        IAsyncEnumerable<T> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        Dictionary<int, List<T>> results = [];

        await AsyncSeriesHelper
            .ForEachAsync(
                contents,
                iterationLimit,
                canceler,
                async item =>
                    _ = await AddToAsync(results, item, comparer, canceler).ConfigureAwait(false)
            )
            .ConfigureAwait(false);

        return results;
    }

    /// <inheritdoc/>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return IterateAsync(cancellationToken).GetAsyncEnumerator(cancellationToken);
    }

    /// <inheritdoc cref="GetAsyncEnumerator"/>
    private async IAsyncEnumerable<T> IterateAsync(
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        foreach (T item in (await _contents.ConfigureAwait(false)).SelectMany(x => x.Value))
        {
            canceler.ThrowIfCancellationRequested();
            yield return item;
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<KeyValuePair<int, T>> ByHashesAsync(CancellationToken canceler)
    {
        return ThisByHashesAsync(canceler);
    }

    /// <inheritdoc cref="ByHashesAsync"/>
    private async IAsyncEnumerable<KeyValuePair<int, T>> ThisByHashesAsync(
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        foreach (KeyValuePair<int, List<T>> row in await _contents.ConfigureAwait(false))
        {
            foreach (T item in row.Value)
            {
                canceler.ThrowIfCancellationRequested();
                yield return new KeyValuePair<int, T>(row.Key, item);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(T item, CancellationToken canceler)
    {
        int hash = await _comparer.GetHashCodeAsync(item, canceler).ConfigureAwait(false);
        if ((await _contents.ConfigureAwait(false)).TryGetValue(hash, out List<T>? data))
        {
            foreach (T found in data)
            {
                if (await _comparer.EqualsAsync(found, item, canceler).ConfigureAwait(false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(T item, CancellationToken canceler)
    {
        return await AddToAsync(await _contents.ConfigureAwait(false), item, _comparer, canceler)
            .ConfigureAwait(false);
    }

    private static async Task<bool> AddToAsync(
        Dictionary<int, List<T>> contents,
        T item,
        IAsyncEqualityComparer<T> comparer,
        CancellationToken canceler
    )
    {
        int hash = await comparer.GetHashCodeAsync(item, canceler).ConfigureAwait(false);
        if (contents.TryGetValue(hash, out List<T>? data))
        {
            foreach (T found in data)
            {
                if (await comparer.EqualsAsync(found, item, canceler).ConfigureAwait(false))
                {
                    return false;
                }
            }
            data.Add(item);
            return true;
        }
        else
        {
            contents.Add(hash, [item]);
            return true;
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<T> FindMatchesInAsync(
        IAsyncSet<T> collection,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(collection);

        return ThisFindMatchesInAsync(collection, canceler);
    }

    /// <inheritdoc cref="FindMatchesInAsync"/>
    private async IAsyncEnumerable<T> ThisFindMatchesInAsync(
        IAsyncSet<T> collection,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        Dictionary<int, List<T>> contents = await _contents.ConfigureAwait(false);

        await foreach (
            KeyValuePair<int, T> item in collection.ByHashesAsync(canceler).ConfigureAwait(false)
        )
        {
            if (contents.TryGetValue(item.Key, out List<T>? match))
            {
                foreach (T found in match)
                {
                    if (
                        await _comparer
                            .EqualsAsync(item.Value, found, canceler)
                            .ConfigureAwait(false)
                    )
                    {
                        yield return found;
                    }
                }
            }
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<T> FindMissingFromAsync(
        IAsyncSet<T> collection,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(collection);

        return ThisFindMissingFromAsync(collection, canceler);
    }

    /// <inheritdoc cref="FindMissingFromAsync"/>
    private async IAsyncEnumerable<T> ThisFindMissingFromAsync(
        IAsyncSet<T> collection,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        Dictionary<int, List<T>> contents = await _contents.ConfigureAwait(false);

        await foreach (
            KeyValuePair<int, T> item in collection.ByHashesAsync(canceler).ConfigureAwait(false)
        )
        {
            bool missing = true;

            if (contents.TryGetValue(item.Key, out List<T>? match))
            {
                foreach (T found in match)
                {
                    if (
                        await _comparer
                            .EqualsAsync(item.Value, found, canceler)
                            .ConfigureAwait(false)
                    )
                    {
                        missing = false;
                        break;
                    }
                }
            }

            if (missing)
            {
                yield return item.Value;
            }
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (!_contents.IsCompleted)
        {
            return "AsyncHashSet: { 'Contents currently unresolved' }";
        }

#pragma warning disable VSTHRD002, VSTHRD104 // Completion verified.
        Dictionary<int, List<T>> contents = _contents.Result;
#pragma warning restore

        StringBuilder text = new();

        text.AppendLine("AsyncHashSet: {");
        foreach (KeyValuePair<int, List<T>> row in contents)
        {
            foreach (T item in row.Value)
            {
                text.Append("    ").Append(row.Key).Append(", ").Append(item).AppendLine();
            }
        }
        text.Append('}');

        return text.ToString();
    }
}
