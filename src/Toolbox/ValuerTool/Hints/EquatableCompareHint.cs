using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IEquatable{T}"/> instances for <see cref="IValuer"/>.</summary>
public sealed class EquatableCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.Options.UseEquatableComparisons
            && expected != null
            && expected is not IStructuralEquatable
            && expected is not IToolOptions
            && expected
                .GetType()
                .Inherits(typeof(IEquatable<>).MakeGenericType(expected.GetType()));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));

        if (!expected.Equals(actual))
        {
            return [new Difference(expected, actual)];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));

        return item.GetHashCode();
    }
}
