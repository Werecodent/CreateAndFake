using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
public static class AsyncEnumHelper
{
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
        ArgumentGuard.ThrowIfNull(canceler, nameof(canceler));

        if (values == null)
        {
            return false;
        }
        await foreach (T _ in values.ConfigureAwait(false).WithCancellation(canceler))
        {
            return true;
        }

        canceler.ThrowIfCancellationRequested();
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
        List<T> results = [];
        if (values != null)
        {
            await foreach (T value in values.WithCancellation(canceler).ConfigureAwait(false))
            {
                results.Add(value);
            }
        }
        return results;
    }
}
