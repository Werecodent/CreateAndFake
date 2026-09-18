using System.Collections;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.ExtractorTool;

/// <summary>Extracted content of an object.</summary>
/// <param name="content"><inheritdoc cref="_content" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="_options" path="/summary"/></param>
public sealed class AsyncContentMap(IAsyncSet<object> content, ExtractorOptions options)
    : IAsyncContentMap
{
    /// <summary>Flattened object data.</summary>
    private readonly IAsyncSet<object> _content =
        content ?? throw new ArgumentNullException(nameof(content));

    /// <summary>Configured options used to extract the contents.</summary>
    private readonly ExtractorOptions _options =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IAsyncEnumerable<object> AllContentAsync(CancellationToken canceler)
    {
        return GetAllContentAsync(canceler);
    }

    /// <inheritdoc cref="AllContentAsync"/>
    private async IAsyncEnumerable<object> GetAllContentAsync(
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        await foreach (object item in _content.WithCancellation(canceler).ConfigureAwait(false))
        {
            canceler.ThrowIfCancellationRequested();
            yield return item;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> HasContentAsync(object? item, CancellationToken canceler)
    {
        if (item == null)
        {
            return false;
        }

        return await _content.ContainsAsync(item, canceler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task<bool> HasSharedContentAsync(IAsyncContentMap map, CancellationToken canceler)
    {
        return HasSharedContentAsync([map], canceler);
    }

    /// <inheritdoc/>
    public async Task<bool> HasSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    )
    {
        await foreach (object _ in FindSharedContentAsync(maps, canceler).ConfigureAwait(false))
        {
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<object> FindSharedContentAsync(
        IAsyncContentMap map,
        CancellationToken canceler
    )
    {
        return FindSharedContentAsync([map], canceler);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<object> FindSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        CancellationToken canceler
    )
    {
        return GetSharedContentAsync(maps, canceler);
    }

    /// <inheritdoc cref="FindSharedContentAsync(IEnumerable{IAsyncContentMap},CancellationToken)"/>
    private async IAsyncEnumerable<object> GetSharedContentAsync(
        IEnumerable<IAsyncContentMap> maps,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        if (maps == null)
        {
            yield break;
        }

        await foreach (object item in GetAllIntersectsAsync(maps, canceler).ConfigureAwait(false))
        {
            if (item == null)
            {
                continue;
            }

            Type itemType = item.GetType();

            if (!_options.UniqueIgnoredTypes.Contains(itemType) && !itemType.IsEnum)
            {
                if (item is IEnumerable series)
                {
                    foreach (object _ in series)
                    {
                        yield return item;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }

    /// <inheritdoc cref="GetSharedContentAsync"/>
    private async IAsyncEnumerable<object> GetAllIntersectsAsync(
        IEnumerable<IAsyncContentMap> maps,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        if (maps == null)
        {
            yield break;
        }

        await foreach (
            KeyValuePair<int, object> entry in _content
                .ByHashesAsync(canceler)
                .ConfigureAwait(false)
        )
        {
            foreach (IAsyncContentMap map in maps)
            {
                if (map == null)
                {
                    continue;
                }

                if (await map.HasContentAsync(entry.Value, canceler).ConfigureAwait(false))
                {
                    yield return entry.Value;
                }
            }
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<T> FindAllAsync<T>(CancellationToken canceler)
    {
        return GetAllAsync<T>(canceler);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<object> FindAllAsync(Type type, CancellationToken canceler)
    {
        return GetAllAsync(type, canceler);
    }

    /// <inheritdoc cref="FindAllAsync"/>
    private async IAsyncEnumerable<T> GetAllAsync<T>(
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        await foreach (object result in GetAllAsync(typeof(T), canceler).ConfigureAwait(false))
        {
            yield return (T)result;
        }
    }

    /// <inheritdoc cref="FindAllAsync(Type,CancellationToken)"/>
    private async IAsyncEnumerable<object> GetAllAsync(
        Type type,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        await foreach (object item in _content.WithCancellation(canceler).ConfigureAwait(false))
        {
            if (item?.GetType().Inherits(type) ?? false)
            {
                yield return item;
            }
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"ContentMap: {_content}";
    }
}
