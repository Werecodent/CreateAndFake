using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="MemberInfo"/> instances for <see cref="IValuer"/>.</summary>
public sealed class MemberInfoCompareHint : CompareHint<MemberInfo>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        MemberInfo? expected,
        MemberInfo? actual,
        ValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (
            expected.MetadataToken != actual.MetadataToken
            || expected.Module.MetadataToken != actual.Module.MetadataToken
        )
        {
            return [new Difference(expected, actual)];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(MemberInfo? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode((item.MetadataToken, item.Module.MetadataToken));
    }
}
