using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <typeparam name="T"><c>Type</c> being supported for comparisons.</typeparam>
/// <inheritdoc/>
public abstract class CompareHint<T> : CompareHint
{
    /// <inheritdoc/>
    protected sealed override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));

        return expected is T && actual is T;
    }

    /// <inheritdoc/>
    protected sealed override IEnumerable<Difference> Compare(object? expected, object? actual, ValuerChainer valuer)
    {
        return Compare((T?)expected, (T?)actual, valuer);
    }

    /// <inheritdoc cref="Compare(object,object,ValuerChainer)"/>
    protected abstract IEnumerable<Difference> Compare(T? expected, T? actual, ValuerChainer valuer);

    /// <inheritdoc/>
    protected sealed override int GetHashCode(object? item, ValuerChainer valuer)
    {
        return GetHashCode((T?)item, valuer);
    }

    /// <inheritdoc cref="GetHashCode(object,ValuerChainer)"/>
    protected abstract int GetHashCode(T? item, ValuerChainer valuer);
}
