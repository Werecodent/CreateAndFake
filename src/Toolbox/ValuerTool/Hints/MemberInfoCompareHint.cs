using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="MemberInfo"/> instances for <see cref="IValuer"/>.</summary>
public sealed class MemberInfoCompareHint : CompareHint<MemberInfo>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        MemberInfo? expected,
        MemberInfo? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected);
        ArgumentGuard.ThrowIfNull(actual);
        ArgumentGuard.ThrowIfNull(valuer);

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
    protected override int GetHashCode(MemberInfo? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item);
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode((item.MetadataToken, item.Module.MetadataToken));
    }
}
