namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <typeparam name="T"><see cref="Type"/> being supported for comparisons.</typeparam>
/// <inheritdoc/>
public abstract class CompareHint<T> : CompareHint
{
    /// <inheritdoc/>
    protected sealed override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        return expected is T && actual is T;
    }

    /// <inheritdoc/>
    protected sealed override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerChainer valuer
    )
    {
        return Compare((T?)expected, (T?)actual, valuer);
    }

    /// <inheritdoc cref="Compare(object,object,ValuerChainer)"/>
    protected abstract IEnumerable<Difference> Compare(
        T? expected,
        T? actual,
        ValuerChainer valuer
    );

    /// <inheritdoc/>
    protected sealed override Task<IEnumerable<Difference>> CompareAsync(
        object? expected,
        object? actual,
        ValuerChainer valuer
    )
    {
        return CompareAsync((T?)expected, (T?)actual, valuer);
    }

    /// <inheritdoc cref="CompareAsync(object,object,ValuerChainer)"/>
    protected virtual Task<IEnumerable<Difference>> CompareAsync(
        T? expected,
        T? actual,
        ValuerChainer valuer
    )
    {
        return Task.FromResult<IEnumerable<Difference>>([.. Compare(expected, actual, valuer)]);
    }

    /// <inheritdoc/>
    protected sealed override int GetHashCode(object? item, ValuerChainer valuer)
    {
        return GetHashCode((T?)item, valuer);
    }

    /// <inheritdoc cref="GetHashCode(object,ValuerChainer)"/>
    protected abstract int GetHashCode(T? item, ValuerChainer valuer);

    /// <inheritdoc/>
    protected sealed override Task<int> GetHashCodeAsync(object? item, ValuerChainer valuer)
    {
        return GetHashCodeAsync((T?)item, valuer);
    }

    /// <inheritdoc cref="GetHashCodeAsync(object,ValuerChainer)"/>
    protected virtual Task<int> GetHashCodeAsync(T? item, ValuerChainer valuer)
    {
        return Task.FromResult(GetHashCode(item, valuer));
    }
}

#pragma warning restore MA0042
