using CreateAndFake.Design;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IFaked"/> instances for <see cref="IValuer"/>.</summary>
public sealed class FakedCompareHint : CompareHint<IFaked>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.FakedHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IFaked? expected,
        IFaked? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.Compare(expected?.FakeMeta, actual?.FakeMeta);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IFaked? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode(item?.FakeMeta);
    }
}
