using CreateAndFake.Design.Content;

namespace CreateAndFake.DuplicatorTool;

/// <inheritdoc/>
public sealed class CopyHintResult : HintResult<object?>
{
    /// <summary>For when a hint doesn't support a type or fails to copy it.</summary>
    public static CopyHintResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private CopyHintResult(bool hasData, object? data) : base(hasData, data) { }

    /// <inheritdoc cref="CopyHintResult(bool,object)"/>
    public CopyHintResult(object? data) : this(true, data) { }
}
