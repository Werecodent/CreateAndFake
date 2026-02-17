using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances needing to use regular equality/hashing for <see cref="IValuer"/>.</summary>
public sealed class FallbackCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.FallbackHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer valuer)
    {
        return valuer.Options.FallbackTypes.Contains(expected.GetType());
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        if (expected != actual)
        {
            yield return new Difference(".equals", new Difference(expected, actual));
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer valuer)
    {
        return item.GetHashCode();
    }
}
