using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="ParameterInfo"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ParameterInfoCompareHint : CompareHint<ParameterInfo>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        ParameterInfo? expected,
        ParameterInfo? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (expected.MetadataToken != actual.MetadataToken)
        {
            return [new Difference(expected, actual)];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(ParameterInfo? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode(item.MetadataToken);
    }
}
