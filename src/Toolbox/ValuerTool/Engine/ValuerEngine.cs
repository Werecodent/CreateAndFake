using System.Collections.Immutable;
using CreateAndFake.Design;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
/// <param name="defaultHints">Generators used to compare specific types.</param>
public sealed class ValuerEngine(ImmutableArray<CompareHint> defaultHints) : IValuerEngine
{
    /// <summary>Picks hints to use for randomization based upon <paramref name="options"/>.</summary>
    /// <param name="options">Potentially modified configuration to use.</param>
    /// <returns>Cached hints if possible; built hints otherwise.</returns>
    private IEnumerable<CompareHint> SelectHints(ValuerOptions options)
    {
        foreach (CompareHint hint in options.Hints)
        {
            yield return hint;
        }
        if (options.IncludeDefaultHints)
        {
            foreach (CompareHint hint in defaultHints)
            {
                yield return hint;
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(object? expected, object? actual, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

        if (ReferenceEquals(expected, actual))
        {
            return [];
        }

        DifferenceHintResult? result = SelectHints(chainer.Options)
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
    public Task<IEnumerable<Difference>> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer chainer
    )
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

        if (ReferenceEquals(expected, actual))
        {
            return Task.FromResult<IEnumerable<Difference>>([]);
        }

        DifferenceHintAsyncResult? result = SelectHints(chainer.Options)
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

        HashCodeHintResult? result = SelectHints(chainer.Options)
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

        HashCodeHintAsyncResult? result = SelectHints(chainer.Options)
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
