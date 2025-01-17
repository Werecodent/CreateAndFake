using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <inheritdoc/>
public sealed class HashCodeHintResult : HintResult<int>
{
    /// <summary>For when a hint doesn't support a type or fails to compare it.</summary>
    public static HashCodeHintResult None { get; } = new(false, default);

    /// <inheritdoc/>
    private HashCodeHintResult(bool hasData, int data) : base(hasData, data) { }

    /// <inheritdoc cref="HashCodeHintResult(bool,int)"/>
    public HashCodeHintResult(int data) : this(true, data) { }
}