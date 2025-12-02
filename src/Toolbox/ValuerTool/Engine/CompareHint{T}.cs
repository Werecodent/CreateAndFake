using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <typeparam name="T"><see cref="Type"/> being supported for comparisons.</typeparam>
/// <inheritdoc/>
public abstract class CompareHint<T> : CompareHint
{
    /// <inheritdoc/>
    protected sealed override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        return expected is T && actual is T;
    }

    /// <inheritdoc/>
    protected sealed override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        return Compare((T?)expected, (T?)actual, valuer);
    }

    /// <inheritdoc cref="Compare(object,object,IValuerChainer)"/>
    protected abstract IEnumerable<Difference> Compare(
        T? expected,
        T? actual,
        IValuerChainer valuer
    );

    /// <inheritdoc/>
    protected sealed override IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        return CompareAsync((T?)expected, (T?)actual, valuer);
    }

    /// <inheritdoc cref="CompareAsync(object,object,IValuerChainer)"/>
    protected virtual IAsyncEnumerable<Difference> CompareAsync(
        T? expected,
        T? actual,
        IValuerChainer valuer
    )
    {
        return AsyncEnumHelper.CreateFrom(Compare(expected, actual, valuer));
    }

    /// <inheritdoc/>
    protected sealed override int GetHashCode(object? item, IValuerChainer valuer)
    {
        return GetHashCode((T?)item, valuer);
    }

    /// <inheritdoc cref="GetHashCode(object,IValuerChainer)"/>
    protected abstract int GetHashCode(T? item, IValuerChainer valuer);

    /// <inheritdoc/>
    protected sealed override Task<int> GetHashCodeAsync(object? item, IValuerChainer valuer)
    {
        return GetHashCodeAsync((T?)item, valuer);
    }

    /// <inheritdoc cref="GetHashCodeAsync(object,IValuerChainer)"/>
    protected virtual Task<int> GetHashCodeAsync(T? item, IValuerChainer valuer)
    {
        return Task.FromResult(GetHashCode(item, valuer));
    }
}

#pragma warning restore MA0042
