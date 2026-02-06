using System.Text;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="StringBuilder"/> instances for <see cref="IValuer"/>.</summary>
public sealed class StringBuilderCompareHint : CompareHint<StringBuilder>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.StringBuilderHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        StringBuilder? expected,
        StringBuilder? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.Compare(expected?.ToString(), actual?.ToString());
    }

    /// <inheritdoc/>
    protected override int GetHashCode(StringBuilder? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode(item?.ToString());
    }
}
