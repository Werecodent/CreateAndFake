using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Provides a repeatable asynchronous source.</summary>
/// <typeparam name="T">The <see cref="Content"/>'s item <see cref="Type"/>.</typeparam>
public sealed class AsyncList<T> : IAsyncEnumerable<T>
{
    /// <summary>Backing content for the async enumerator.</summary>
    public IEnumerable<T> Content { get; }

    /// <inheritdoc cref="AsyncList{T}"/>
    /// <param name="content"><inheritdoc cref="Content" path="/summary"/></param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    public AsyncList(IEnumerable<T> content, int iterationLimit)
    {
        List<T> list = [];

        int i = 0;
        foreach (T item in content ?? throw new ArgumentNullException(nameof(content)))
        {
            ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
            list.Add(item);
        }

        Content = list;
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
        return $"{GenericConverter.ExpandName(GetType())}[{Content.Count()}]";
    }
}
