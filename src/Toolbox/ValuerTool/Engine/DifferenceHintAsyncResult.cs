using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc/>
public sealed class DifferenceHintAsyncResult : HintResult<IAsyncEnumerable<Difference>?>
{
    /// <summary>For when a hint doesn't support a type or fails to compare it.</summary>
    public static DifferenceHintAsyncResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private DifferenceHintAsyncResult(bool hasData, IAsyncEnumerable<Difference>? data)
        : base(hasData, data) { }

    /// <inheritdoc cref="DifferenceHintAsyncResult(bool,IAsyncEnumerable{Difference})"/>
    public DifferenceHintAsyncResult(IAsyncEnumerable<Difference> data)
        : this(true, data) { }
}
