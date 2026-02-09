using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
/// <remarks>Manually required as .NET 10 breaks <c>System.Linq.Async</c> compatibility.</remarks>
public static class AsyncEnumHelper
{
    /// <summary>Delegate for triggering asynchronous cancellation.</summary>
    /// <remarks><see langword="null"/> when unavailable for the executing .NET version.</remarks>
    private static readonly Func<CancellationTokenSource, Task>? _CancelAsyncForCancellationToken =
        (Func<CancellationTokenSource, Task>?)
            typeof(CancellationTokenSource)
                .GetMethod("CancelAsync")
                ?.CreateDelegate(typeof(Func<CancellationTokenSource, Task>));

    /// <summary>
    ///     Converts the <paramref name="collection"/> to an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The <paramref name="collection"/> item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to convert via iteration.</param>
    /// <returns>Asynchronous iteration of the <paramref name="collection"/>.</returns>
    [return: NotNullIfNotNull(nameof(collection))]
    public static IAsyncEnumerable<T>? CreateFrom<T>(IEnumerable<T>? collection)
    {
        if (collection == null)
        {
            return null;
        }
        else
        {
            return IterateAsync(collection);
        }
    }

#pragma warning disable IDE0390 // Purpose is to create async data from sync data.

    /// <inheritdoc cref="CreateFrom{T}"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    private static async IAsyncEnumerable<T> IterateAsync<T>(
        IEnumerable<T> collection,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        foreach (T value in collection)
        {
            canceler.ThrowIfCancellationRequested();
            yield return value;
        }
    }

#pragma warning restore IDE0390

    /// <summary>Determines if the <paramref name="collection"/> has any items.</summary>
    /// <typeparam name="T">The <paramref name="collection"/> item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to check for an item via iteration.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="collection"/>
    ///     has at least one item, <see langword="false"/> otherwise.
    /// </returns>
    public static async Task<bool> HasAnyAsync<T>(
        [NotNullWhen(true)] IAsyncEnumerable<T>? collection,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        canceler.ThrowIfCancellationRequested();
        if (collection == null)
        {
            return false;
        }
        await foreach (T _ in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            return true;
        }

        canceler.ThrowIfCancellationRequested();
        return false;
    }

    /// <summary>Converts the <paramref name="collection"/> to an <see cref="IList{T}"/>.</summary>
    /// <typeparam name="T">The <paramref name="collection"/> item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to convert via iteration.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     Contents of the <paramref name="collection"/> as an <see cref="IList{T}"/>;
    ///     an empty list if the <paramref name="collection"/> is <see langword="null"/>.
    /// </returns>
    public static async Task<IList<T>> ToListAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        canceler.ThrowIfCancellationRequested();

        List<T> results = [];
        if (collection != null)
        {
            await foreach (T value in collection.WithCancellation(canceler).ConfigureAwait(false))
            {
                results.Add(value);
                canceler.ThrowIfCancellationRequested();
            }
        }

        canceler.ThrowIfCancellationRequested();
        return results;
    }

    /// <summary>
    ///     Creates an empty <see cref="IAsyncEnumerable{T}"/> that triggers
    ///     cancellation via the <paramref name="source"/> upon attempted iteration.
    /// </summary>
    /// <returns>An empty asynchronous series.</returns>
    /// <inheritdoc cref="CreateFrom{T}"/>
    /// <inheritdoc cref="TriggerCancellationAsync(CancellationTokenSource)"/>
    public static async IAsyncEnumerable<T> CreateCancelingIteration<T>(
        CancellationTokenSource source
    )
    {
        ArgumentGuard.ThrowIfNull(source);
        await TriggerCancellationAsync(source).ConfigureAwait(false);
        yield break;
    }

    /// <summary>
    ///     Converts the <paramref name="collection"/> to an <see cref="IAsyncEnumerable{T}"/> and
    ///     triggers cancellation via the <paramref name="source"/> after the first yielded value.
    /// </summary>
    /// <returns>Asynchronous iteration of the <paramref name="collection"/>.</returns>
    /// <inheritdoc cref="CreateCancelingIteration{T}(CancellationTokenSource)"/>
    public static async IAsyncEnumerable<T> CreateCancelingIteration<T>(
        IEnumerable<T> collection,
        CancellationTokenSource source
    )
    {
        ArgumentGuard.ThrowIfNull(collection, source);

        bool notCanceled = true;
        foreach (T value in collection)
        {
            yield return value;
            if (notCanceled)
            {
                await TriggerCancellationAsync(source).ConfigureAwait(false);
                notCanceled = false;
            }
        }
    }

    /// <summary>
    ///     Handles canceling a token via its <paramref name="source"/> using async if possible.
    /// </summary>
    /// <param name="source">Owner of the <see cref="CancellationToken"/> to cancel.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>Asynchronous cancellation requires .NET 8 or later.</remarks>
    public static Task TriggerCancellationAsync(CancellationTokenSource source)
    {
        return TriggerCancellationAsync(_CancelAsyncForCancellationToken, source);
    }

    /// <param name="cancelAsyncMethod">Delegate for canceling via async if available.</param>
    /// <inheritdoc cref="TriggerCancellationAsync(CancellationTokenSource)"/>
    internal static async Task TriggerCancellationAsync(
        Func<CancellationTokenSource, Task>? cancelAsyncMethod,
        CancellationTokenSource source
    )
    {
        ArgumentGuard.ThrowIfNull(source);

        if (!source.IsCancellationRequested)
        {
            if (cancelAsyncMethod != null)
            {
                await cancelAsyncMethod(source).ConfigureAwait(false);
            }
            else
            {
#pragma warning disable AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103 // CancelAsync not available.
                source.Cancel();
#pragma warning restore AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103
            }
        }
    }
}
