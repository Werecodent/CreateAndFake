using CreateAndFake.Design;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="IFaked"/> instances for <see cref="IValuer"/>.</summary>
public sealed class FakedCompareHint : CompareHint<IFaked>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(IFaked? expected, IFaked? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.Compare(expected?.FakeMeta, actual?.FakeMeta);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IFaked? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode(item?.FakeMeta);
    }
}
