using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc/>
public sealed class HashCodeHintAsyncResult : HintResult<Task<int>?>
{
    /// <summary>For when a hint doesn't support a type or fails to compare it.</summary>
    public static HashCodeHintAsyncResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private HashCodeHintAsyncResult(bool hasData, Task<int>? data)
        : base(hasData, data) { }

    /// <inheritdoc cref="HashCodeHintAsyncResult(bool,Task{int})"/>
    public HashCodeHintAsyncResult(Task<int> data)
        : this(true, data) { }
}
