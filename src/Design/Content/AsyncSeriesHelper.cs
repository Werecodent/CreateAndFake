using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
/// <remarks>Manually required as .NET 10 breaks <c>System.Linq.Async</c> compatibility.</remarks>
public static class AsyncSeriesHelper
{
    /// <summary>Converts the <paramref name="collection"/> to an <see cref="IAsyncEnumerable{T}"/>.</summary>
    /// <typeparam name="T">The <paramref name="collection"/>'s item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to convert via iteration.</param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Asynchronous iteration of the <paramref name="collection"/>.</returns>
    /// <remarks>The <paramref name="canceler"/> is currently unused; maintained for future compatibility.</remarks>
    [return: NotNullIfNotNull(nameof(collection))]
    public static IAsyncEnumerable<T>? CreateFromAsync<T>(
        IEnumerable<T>? collection,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        if (collection == null)
        {
            return null;
        }
        else
        {
            return new AsyncList<T>(collection, iterationLimit);
        }
    }

    /// <summary>Determines if the <paramref name="collection"/> has any items.</summary>
    /// <typeparam name="T">The <paramref name="collection"/>'s item <see cref="Type"/>.</typeparam>
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

#pragma warning disable CA1068 // Token is relevant to the core functionality for the method.

    /// <summary>
    ///     Safely executes <paramref name="itemHandler"/> on the contents of the <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T">The <paramref name="collection"/>'s item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to iterate.</param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="itemHandler">Behavior to run on each <paramref name="collection"/> item.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>Properly handles cancellation and prevents infinite iteration.</remarks>
    /// <exception cref="IterationLimitException">
    ///     If the <paramref name="collection"/> size is <c>&gt;= iterationLimit</c>.
    /// </exception>
    public static Task ForEachAsync<T>(
        IAsyncEnumerable<T>? collection,
        int iterationLimit,
        CancellationToken canceler,
        Action<T> itemHandler
    )
    {
        ArgumentGuard.ThrowIfNull(itemHandler);
        return ForEachAsync(
            collection,
            iterationLimit,
            canceler,
            item =>
            {
                itemHandler(item);
                return Task.CompletedTask;
            }
        );
    }

    /// <inheritdoc cref="ForEachAsync{T}(IAsyncEnumerable{T},int,CancellationToken,Action{T})"/>
    public static async Task ForEachAsync<T>(
        IAsyncEnumerable<T>? collection,
        int iterationLimit,
        CancellationToken canceler,
        Func<T, Task> itemHandler
    )
    {
        ArgumentGuard.ThrowIfNull(itemHandler);
        if (collection == null)
        {
            return;
        }

        canceler.ThrowIfCancellationRequested();

        int i = 0;
        await foreach (T item in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
            canceler.ThrowIfCancellationRequested();
            await itemHandler(item).ConfigureAwait(false);
        }

        canceler.ThrowIfCancellationRequested();
    }

    /// <inheritdoc cref="HandleSelectAsync"/>
    public static IAsyncEnumerable<TOut> SelectAsync<TIn, TOut>(
        IAsyncEnumerable<TIn>? collection,
        int iterationLimit,
        CancellationToken canceler,
        Func<TIn, TOut> itemHandler
    )
    {
        ArgumentGuard.ThrowIfNull(itemHandler);
        return SelectAsync(
            collection,
            iterationLimit,
            canceler,
            item => Task.FromResult(itemHandler(item))
        );
    }

    /// <inheritdoc cref="HandleSelectAsync"/>
    public static IAsyncEnumerable<TOut> SelectAsync<TIn, TOut>(
        IAsyncEnumerable<TIn>? collection,
        int iterationLimit,
        CancellationToken canceler,
        Func<TIn, Task<TOut>> itemHandler
    )
    {
        return HandleSelectAsync(collection, itemHandler, iterationLimit, canceler);
    }

    /// <summary>Sequentially modifies the <paramref name="collection"/>.</summary>
    /// <typeparam name="TIn">The <paramref name="collection"/>'s item <see cref="Type"/>.</typeparam>
    /// <typeparam name="TOut">The <see langword="return"/> item <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="ForEachAsync{T}(IAsyncEnumerable{T},int,CancellationToken,Action{T})"/>
    private static async IAsyncEnumerable<TOut> HandleSelectAsync<TIn, TOut>(
        IAsyncEnumerable<TIn>? collection,
        Func<TIn, Task<TOut>> itemHandler,
        int iterationLimit,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        ArgumentGuard.ThrowIfNull(itemHandler);
        if (collection == null)
        {
            yield break;
        }

        canceler.ThrowIfCancellationRequested();

        int i = 0;
        await foreach (TIn item in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
            canceler.ThrowIfCancellationRequested();
            yield return await itemHandler(item).ConfigureAwait(false);
        }

        canceler.ThrowIfCancellationRequested();
    }

#pragma warning restore CA1068

    /// <summary>Converts the <paramref name="collection"/> to an <see cref="IList{T}"/>.</summary>
    /// <typeparam name="T">The <paramref name="collection"/>'s item <see cref="Type"/>.</typeparam>
    /// <param name="collection">Series to convert via iteration.</param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     Contents of the <paramref name="collection"/> as an <see cref="IList{T}"/>;
    ///     an empty list if the <paramref name="collection"/> is <see langword="null"/>.
    /// </returns>
    public static async Task<IList<T>> ToListAsync<T>(
        IAsyncEnumerable<T>? collection,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        canceler.ThrowIfCancellationRequested();

        List<T> results = [];
        if (collection != null)
        {
            int i = 0;
            await foreach (T value in collection.WithCancellation(canceler).ConfigureAwait(false))
            {
                ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
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
    /// <inheritdoc cref="TriggerCancellationAsync(CancellationTokenSource)"/>
    public static async IAsyncEnumerable<T> CreateCancelingIterationAsync<T>(
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
    /// <inheritdoc cref="CreateCancelingIterationAsync{T}(CancellationTokenSource)"/>
    public static async IAsyncEnumerable<T> CreateCancelingIterationAsync<T>(
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

    /// <inheritdoc cref="MaybeAsyncCanceler.TriggerCancellationAsync"/>
    public static Task TriggerCancellationAsync(CancellationTokenSource source)
    {
        return MaybeAsyncCanceler.Use.TriggerCancellationAsync(source);
    }
}
