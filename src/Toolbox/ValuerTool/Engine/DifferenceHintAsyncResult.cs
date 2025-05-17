using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc/>
public sealed class DifferenceHintAsyncResult : HintResult<Task<IEnumerable<Difference>>?>
{
    /// <summary>For when a hint doesn't support a type or fails to compare it.</summary>
    public static DifferenceHintAsyncResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private DifferenceHintAsyncResult(bool hasData, Task<IEnumerable<Difference>>? data)
        : base(hasData, data) { }

    /// <inheritdoc cref="DifferenceHintAsyncResult(bool,Task{IEnumerable{Difference}})"/>
    public DifferenceHintAsyncResult(Task<IEnumerable<Difference>> data)
        : this(true, data) { }
}
