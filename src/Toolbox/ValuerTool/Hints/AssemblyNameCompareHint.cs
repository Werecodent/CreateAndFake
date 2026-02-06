using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="AssemblyName"/> instances for <see cref="IValuer"/>.</summary>
public sealed class AssemblyNameCompareHint : CompareHint<AssemblyName>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.AssemblyNameHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        AssemblyName? expected,
        AssemblyName? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.Compare(expected?.FullName, actual?.FullName);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(AssemblyName? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode(item?.FullName);
    }
}
