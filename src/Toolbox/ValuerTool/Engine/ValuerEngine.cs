using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
/// <param name="defaultHints">Generators used to compare specific types.</param>
public sealed class ValuerEngine(IEnumerable<CompareHint> defaultHints)
    : ToolEngine<CompareHint>(defaultHints),
        IValuerEngine
{
    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(object? expected, object? actual, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

        if (ReferenceEquals(expected, actual))
        {
            return [];
        }

        DifferenceHintResult? result = SelectHints(chainer)
            .Select(h => h.TryCompare(expected, actual, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{expected?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
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
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

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
                $"Type '{expected?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

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
                $"Type '{item?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public Task<int> GetHashCodeAsync(object? item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

        HashCodeHintAsyncResult? result = SelectHints(chainer)
            .Select(h => h.TryAsyncGetHashCode(item, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{item?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }
}
