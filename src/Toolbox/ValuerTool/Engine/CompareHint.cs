using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <inheritdoc cref="ICompareHint"/>
public abstract class CompareHint : ICompareHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <inheritdoc/>
    public DifferenceHintResult TryCompare(object expected, object actual, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(expected, actual, chainer);

        if (Supports(expected, actual, chainer))
        {
            IEnumerable<Difference> results = Compare(expected, actual, chainer);
            if (chainer.Options.IncludeValueHashInComparison)
            {
                int expectedHash = GetHashCode(expected, chainer);
                int actualHash = GetHashCode(actual, chainer);

                if (expectedHash != actualHash)
                {
                    results = results.Append(
                        new Difference("(ValueHash)", new Difference(expectedHash, actualHash))
                    );
                }
            }
            return new(results);
        }
        else
        {
            return DifferenceHintResult.None;
        }
    }

    /// <inheritdoc/>
    public DifferenceHintAsyncResult TryAsyncCompare(
        object expected,
        object actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(expected, actual, chainer);

        if (Supports(expected, actual, chainer))
        {
            return new(HandleCompareAsync(expected, actual, chainer, canceler));
        }
        else
        {
            return DifferenceHintAsyncResult.None;
        }
    }

    /// <inheritdoc cref="TryAsyncCompare"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    private async IAsyncEnumerable<Difference> HandleCompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        await foreach (
            Difference diff in CompareAsync(expected, actual, chainer, canceler)
                .ConfigureAwait(false)
        )
        {
            canceler.ThrowIfCancellationRequested();
            yield return diff;
        }

        if (chainer.Options.IncludeValueHashInComparison)
        {
            canceler.ThrowIfCancellationRequested();
            int expectedHash = await GetHashCodeAsync(expected, chainer, canceler)
                .ConfigureAwait(false);

            canceler.ThrowIfCancellationRequested();
            int actualHash = await GetHashCodeAsync(actual, chainer, canceler)
                .ConfigureAwait(false);

            if (expectedHash != actualHash)
            {
                yield return new Difference(
                    "(ValueHash)",
                    new Difference(expectedHash, actualHash)
                );
            }
        }
    }

    /// <inheritdoc/>
    public HashCodeHintResult TryGetHashCode(object item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(item, chainer);

        if (Supports(item, item, chainer))
        {
            return new(GetHashCode(item, chainer));
        }
        else
        {
            return HashCodeHintResult.None;
        }
    }

    /// <inheritdoc/>
    public HashCodeHintAsyncResult TryAsyncGetHashCode(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(item, chainer);

        if (Supports(item, item, chainer))
        {
            return new(GetHashCodeAsync(item, chainer, canceler));
        }
        else
        {
            return HashCodeHintAsyncResult.None;
        }
    }

    /// <summary>
    ///     Determines if <paramref name="expected"/> or <paramref name="actual"/> are supported by the hint.
    /// </summary>
    /// <returns><see langword="true"/> if the objects can be compared, <see langword="false"/> otherwise.</returns>
    /// <inheritdoc cref="TryCompare"/>
    protected abstract bool Supports(object expected, object actual, IValuerChainer chainer);

    /// <summary>Finds the differences between <paramref name="expected"/> and <paramref name="actual"/>.</summary>
    /// <returns>The found differences between <paramref name="expected"/> and <paramref name="actual"/>.</returns>
    /// <inheritdoc cref="TryCompare"/>
    protected abstract IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    );

    /// <inheritdoc cref="Compare"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected virtual IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return AsyncEnumHelper.CreateFromAsync(Compare(expected, actual, chainer), canceler);
    }

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <inheritdoc cref="TryGetHashCode"/>
    protected abstract int GetHashCode(object item, IValuerChainer chainer);

    /// <inheritdoc cref="GetHashCode"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected virtual Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return Task.FromResult(GetHashCode(item, chainer));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}

#pragma warning restore MA0042 // Using sync behavior for async versions.
