using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <summary>Handles <typeparamref name="T"/> comparisons.</summary>
/// <typeparam name="T">Specific <see cref="Type"/> the hint supports.</typeparam>
public abstract class CompareHint<T> : CompareHint
{
    /// <inheritdoc/>
    protected sealed override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        return expected is T && actual is T;
    }

    /// <inheritdoc/>
    protected sealed override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        return Compare((T)expected, (T)actual, chainer);
    }

    /// <inheritdoc cref="Compare(object,object,IValuerChainer)"/>
    protected abstract IEnumerable<Difference> Compare(
        T expected,
        T actual,
        IValuerChainer chainer
    );

    /// <inheritdoc/>
    protected sealed override IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return CompareAsync((T)expected, (T)actual, chainer, canceler);
    }

    /// <inheritdoc cref="CompareAsync(object,object,IValuerChainer,CancellationToken)"/>
    protected virtual IAsyncEnumerable<Difference> CompareAsync(
        T expected,
        T actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return AsyncSeriesHelper.CreateFromAsync(Compare(expected, actual, chainer), canceler);
    }

    /// <inheritdoc/>
    protected sealed override int GetHashCode(object item, IValuerChainer chainer)
    {
        return GetHashCode((T)item, chainer);
    }

    /// <inheritdoc cref="GetHashCode(object,IValuerChainer)"/>
    protected abstract int GetHashCode(T item, IValuerChainer chainer);

    /// <inheritdoc/>
    protected sealed override Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return GetHashCodeAsync((T)item, chainer, canceler);
    }

    /// <inheritdoc cref="GetHashCodeAsync(object,IValuerChainer,CancellationToken)"/>
    protected virtual Task<int> GetHashCodeAsync(
        T item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return Task.FromResult(GetHashCode(item, chainer));
    }
}

#pragma warning restore MA0042
