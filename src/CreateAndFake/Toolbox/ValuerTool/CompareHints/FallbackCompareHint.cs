namespace CreateAndFake.Toolbox.ValuerTool.CompareHints;

/// <summary>Handles comparing instances needing to use regular equality/hashing for <see cref="IValuer"/>.</summary>
public sealed class FallbackCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(object? expected, object? actual, ValuerChainer valuer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, ValuerChainer valuer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        throw new NotImplementedException();
    }

}
