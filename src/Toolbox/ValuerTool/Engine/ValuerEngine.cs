using System.Collections.Immutable;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Test</summary>
/// <param name="hints"></param>
public sealed class ValuerEngine(ImmutableArray<CompareHint> hints)
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.Compare(object,object,ValuerMod)"/>
    public IEnumerable<Difference> Compare(object? expected, object? actual, ValuerChainer chainer)
    {
        if (ReferenceEquals(expected, actual))
        {
            return [];
        }

        DifferenceHintResult? result = hints
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

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCodeAsync"/>
    public Task<IEnumerable<Difference>> CompareAsync(
        object? expected,
        object? actual,
        ValuerChainer chainer
    )
    {
        if (ReferenceEquals(expected, actual))
        {
            return Task.FromResult<IEnumerable<Difference>>([]);
        }

        DifferenceHintAsyncResult? result = hints
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

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCode(object)"/>
    public int GetHashCode(object? item, ValuerChainer chainer)
    {
        HashCodeHintResult? result = hints
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

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCodeAsync"/>
    public Task<int> GetHashCodeAsync(object? item, ValuerChainer chainer)
    {
        HashCodeHintAsyncResult? result = hints
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
