using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="MemberInfo"/> collections for <see cref="IValuer"/>.</summary>
public sealed class MemberInfoCompareHint : CompareHint<MemberInfo>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        MemberInfo? expected,
        MemberInfo? actual,
        ValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.Compare($"{expected}", $"{actual}");
    }

    /// <inheritdoc/>
    protected override int GetHashCode(MemberInfo? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode($"{item}");
    }
}
