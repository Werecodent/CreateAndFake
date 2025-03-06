using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool.CompareHints;

/// <summary>Handles comparing instances needing to use regular equality/hashing for <see cref="IValuer"/>.</summary>
public sealed class FallbackCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerChainer valuer
    )
    {
        if (expected != actual)
        {
            return [new Difference(".equals", new Difference(expected, actual))];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, ValuerChainer valuer)
    {
        return item?.GetHashCode() ?? ValueComparer.NullHash;
    }

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        Type? type = (expected ?? actual)?.GetType();

        return type != null && valuer.Options.FallbackTypes.Contains(type);
    }
}
