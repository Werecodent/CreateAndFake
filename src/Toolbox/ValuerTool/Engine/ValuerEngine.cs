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

        DifferenceHintAsyncResult? result = SelectHints(chainer)
            .Select(h => h.TryAsyncCompare(expected, actual, chainer))
            .FirstOrDefault(r => r.HasData);

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

        HashCodeHintResult? result = SelectHints(chainer)
            .Select(h => h.TryGetHashCode(item, chainer))
            .FirstOrDefault(r => r.HasData);

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

        HashCodeHintAsyncResult? result = SelectHints(chainer)
            .Select(h => h.TryAsyncGetHashCode(item, chainer, canceler))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(item?.GetType())}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }
}
