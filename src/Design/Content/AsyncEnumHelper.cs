using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
public static class AsyncEnumHelper
{
    /// <summary>Asynchronous cancellation not available in all versions.</summary>
    private static readonly bool _CanAsyncCancel =
        typeof(CancellationTokenSource).GetMethod("CancelAsync") != null;

    /// <summary>Converts <paramref name="values"/> to an async collection.</summary>
    /// <typeparam name="T">Collection content type.</typeparam>
    /// <param name="values">Collection to convert.</param>
    /// <returns>The converted collection.</returns>
    [return: NotNullIfNotNull(nameof(values))]
    public static IAsyncEnumerable<T>? CreateFrom<T>(IEnumerable<T>? values)
    {
        if (values == null)
        {
            return null;
        }
        else
        {
            return IterateAsync(values);
        }
    }

    /// <inheritdoc cref="CreateFrom{T}"/>
    private static async IAsyncEnumerable<T> IterateAsync<T>(
        IEnumerable<T> values,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        foreach (T value in values)
        {
            canceler.ThrowIfCancellationRequested();
            yield return value;
        }
    }

    /// <summary>Determines if <paramref name="values"/> has any elements.</summary>
    /// <typeparam name="T">Collection content type.</typeparam>
    /// <param name="values">Collection to check.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="values"/> has
    ///     at least one element, <see langword="false"/> otherwise.
    /// </returns>
    public static async Task<bool> HasAnyAsync<T>(
        IAsyncEnumerable<T>? values,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        canceler.ThrowIfCancellationRequested();

        if (values == null)
        {
            return false;
        }
        await foreach (T _ in values.WithCancellation(canceler).ConfigureAwait(false))
        {
            return true;
        }

        return false;
    }

    /// <summary>Converts <paramref name="values"/> to a list.</summary>
    /// <typeparam name="T">Content type.</typeparam>
    /// <param name="values">Collection to convert.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The created list.</returns>
    public static async Task<IList<T>> ToListAsync<T>(
        IAsyncEnumerable<T>? values,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        List<T> results = [];
        if (values != null)
        {
            await foreach (T value in values.WithCancellation(canceler).ConfigureAwait(false))
            {
                results.Add(value);
                canceler.ThrowIfCancellationRequested();
            }
        }
        return results;
    }

    /// <summary>
    ///     Converts <paramref name="values"/> to an async collection but triggers
    ///     cancellation via <paramref name="source"/> after the first yielded value.
    /// </summary>
    /// <typeparam name="T">Collection content type.</typeparam>
    /// <param name="values">Collection to convert.</param>
    /// <param name="source">Source of the cancellation token to cancel.</param>
    /// <returns>The converted collection.</returns>
    public static async IAsyncEnumerable<T> CreateCancelingIteration<T>(
        IEnumerable<T> values,
        CancellationTokenSource source
    )
    {
        ArgumentGuard.ThrowIfNull(values, source);

        foreach (T value in values)
        {
            yield return value;
            if (!source.IsCancellationRequested)
            {
                await TriggerCancellationAsync(source).ConfigureAwait(false);
            }
        }
    }

    /// <summary>Handles canceling via <paramref name="source"/> using async if possible.</summary>
    /// <param name="source">Source of the cancellation token to cancel.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task TriggerCancellationAsync(CancellationTokenSource source)
    {
        ArgumentGuard.ThrowIfNull(source);

        if (_CanAsyncCancel)
        {
            await ((dynamic)source).CancelAsync().ConfigureAwait(false);
        }
        else
        {
#pragma warning disable AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103 // CancelAsync not available.
            source.Cancel();
#pragma warning restore AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103 // CancelAsync not available.
        }
    }
}
