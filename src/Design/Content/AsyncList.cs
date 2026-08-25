using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Provides a repeatable asynchronous source.</summary>
/// <typeparam name="T">The <paramref name="content"/>'s item <see cref="Type"/>.</typeparam>
/// <param name="content"><inheritdoc cref="Content" path="/summary"/></param>
public sealed class AsyncList<T>(IReadOnlyCollection<T> content) : IAsyncEnumerable<T>
{
    /// <summary>Backing content for the async enumerator.</summary>
    public IReadOnlyCollection<T> Content { get; } =
        content.ToArray() ?? throw new ArgumentNullException(nameof(content));

    /// <inheritdoc/>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return IterateContentAsync(cancellationToken).GetAsyncEnumerator(cancellationToken);
    }

    /// <summary>Supplies collection items asynchronously.</summary>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The collection made from <see cref="Content"/>.</returns>
    private async IAsyncEnumerable<T> IterateContentAsync(
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        foreach (T item in Content)
        {
            canceler.ThrowIfCancellationRequested();
            await Task.Delay(0, canceler).ConfigureAwait(false);
            yield return item;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
