using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
public sealed class ValuerEngine : ToolEngine<ICompareHint>, IValuerEngine
{
    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(object? expected, object? actual, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (ReferenceEquals(expected, actual))
        {
            return [];
        }
        else if (expected is null || actual is null)
        {
            return [new Difference(expected, actual)];
        }

        DifferenceHintResult? result = SelectHints(chainer)
            .Select(h => h.TryCompare(expected, actual, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(expected?.GetType())}' not supported by the "
                    + "valuer. Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer chainer
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (ReferenceEquals(expected, actual))
        {
            return AsyncEnumHelper<Difference>.Empty;
        }
        else if (expected is null || actual is null)
        {
            return AsyncEnumHelper.CreateFrom([new Difference(expected, actual)]);
        }

        DifferenceHintAsyncResult? result = SelectHints(chainer)
            .Select(h => h.TryAsyncCompare(expected, actual, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(expected?.GetType())}' not supported by the "
                    + "valuer. Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (item is null)
        {
            return ValueComparer.NullHash;
        }

        HashCodeHintResult? result = SelectHints(chainer)
            .Select(h => h.TryGetHashCode(item, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(item?.GetType())}' not supported by the valuer."
                    + " Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public Task<int> GetHashCodeAsync(
        object? item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (item is null)
        {
            return Task.FromResult(ValueComparer.NullHash);
        }

        HashCodeHintAsyncResult? result = SelectHints(chainer)
            .Select(h => h.TryAsyncGetHashCode(item, chainer, canceler))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(item?.GetType())}' not supported by the valuer."
                    + " Create a hint to generate the type and pass it to the valuer."
            );
        }
    }
}
