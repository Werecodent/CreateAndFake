using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc/>
public sealed class DifferenceHintResult : HintResult<IEnumerable<Difference>?>
{
    /// <summary>For when a hint doesn't support a type or fails to compare it.</summary>
    public static DifferenceHintResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private DifferenceHintResult(bool hasData, IEnumerable<Difference>? data)
        : base(hasData, data) { }

    /// <inheritdoc cref="DifferenceHintResult(bool,IEnumerable{Difference})"/>
    public DifferenceHintResult(IEnumerable<Difference> data)
        : this(true, data) { }
}
