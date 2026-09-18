using System.Runtime.CompilerServices;
using System.Text;
using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.Design.Content;

/// <inheritdoc cref="IAsyncSet{T}"/>
public sealed class AsyncHashSet<T> : IAsyncSet<T>
{
    /// <summary>Current contents for the set.</summary>
    private readonly Task<IDictionary<int, IList<T>>> _contents;

    /// <inheritdoc/>
    public IAsyncEqualityComparer<T> Comparer { get; }

    /// <summary>Creates an empty set.</summary>
    /// <param name="comparer"><inheritdoc cref="Comparer" path="/summary"/></param>
    public AsyncHashSet(IAsyncEqualityComparer<T> comparer)
        : this(
            Task.FromResult<IDictionary<int, IList<T>>>(new Dictionary<int, IList<T>>()),
            comparer
        ) { }

    /// <summary>Creates a populated set.</summary>
    /// <param name="contents"><inheritdoc cref="_contents" path="/summary"/></param>
    /// <param name="comparer"><inheritdoc cref="Comparer" path="/summary"/></param>
    internal AsyncHashSet(
        Task<IDictionary<int, IList<T>>> contents,
        IAsyncEqualityComparer<T> comparer
    )
    {
        ArgumentGuard.ThrowIfNull(contents, comparer);

        Comparer = comparer;
        _contents = contents;
    }

    /// <inheritdoc/>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return IterateAsync(cancellationToken).GetAsyncEnumerator(cancellationToken);
    }

    /// <inheritdoc cref="GetAsyncEnumerator"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <remarks>Visible for testing.</remarks>
    internal async IAsyncEnumerable<T> IterateAsync(
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
        foreach (KeyValuePair<int, IList<T>> row in await _contents.ConfigureAwait(false))
        {
            foreach (T item in row.Value)
            {
                canceler.ThrowIfCancellationRequested();
                yield return new KeyValuePair<int, T>(row.Key, item);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsKeyAsync(int key, CancellationToken canceler)
    {
        return (await _contents.ConfigureAwait(false)).ContainsKey(key);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(T item, CancellationToken canceler)
    {
        int hash = await Comparer.GetHashCodeAsync(item, canceler).ConfigureAwait(false);

        return await ContainsAsync(new KeyValuePair<int, T>(hash, item), canceler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(KeyValuePair<int, T> entry, CancellationToken canceler)
    {
        if ((await _contents.ConfigureAwait(false)).TryGetValue(entry.Key, out IList<T>? data))
        {
            foreach (T found in data)
            {
                if (await Comparer.EqualsAsync(found, entry.Value, canceler).ConfigureAwait(false))
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
        return await AddToAsync(await _contents.ConfigureAwait(false), item, Comparer, canceler)
            .ConfigureAwait(false);
    }

    internal static async Task<bool> AddToAsync(
        IDictionary<int, IList<T>> contents,
        T item,
        IAsyncEqualityComparer<T> comparer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(contents, comparer);

        int hash = await comparer.GetHashCodeAsync(item, canceler).ConfigureAwait(false);
        if (contents.TryGetValue(hash, out IList<T>? data))
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
        IDictionary<int, IList<T>> contents = await _contents.ConfigureAwait(false);

        await foreach (
            KeyValuePair<int, T> item in collection.ByHashesAsync(canceler).ConfigureAwait(false)
        )
        {
            if (contents.TryGetValue(item.Key, out IList<T>? match))
            {
                foreach (T found in match)
                {
                    if (
                        await Comparer
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
        IDictionary<int, IList<T>> contents = await _contents.ConfigureAwait(false);

        await foreach (
            KeyValuePair<int, T> item in collection.ByHashesAsync(canceler).ConfigureAwait(false)
        )
        {
            bool missing = true;

            if (contents.TryGetValue(item.Key, out IList<T>? match))
            {
                foreach (T found in match)
                {
                    if (
                        await Comparer
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
        IDictionary<int, IList<T>> contents = _contents.Result;
#pragma warning restore

        if (contents.Count == 0)
        {
            return "AsyncHashSet: { }";
        }

        StringBuilder text = new();

        text.Append("AsyncHashSet: {");
        foreach (KeyValuePair<int, IList<T>> row in contents)
        {
            foreach (T item in row.Value)
            {
                text.AppendLine().Append("    ").Append(row.Key).Append(", ").Append(item);
            }
        }
        text.AppendLine().Append('}');

        return text.ToString();
    }
}
