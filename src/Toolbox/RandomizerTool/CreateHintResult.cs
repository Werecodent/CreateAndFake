using CreateAndFake.Design.Content;

namespace CreateAndFake.RandomizerTool;

/// <inheritdoc/>
public sealed class CreateHintResult : HintResult<object?>
{
    /// <summary>For when a hint doesn't support a type or fails to create it.</summary>
    public static CreateHintResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private CreateHintResult(bool hasData, object? data) : base(hasData, data) { }

    /// <inheritdoc cref="CreateHintResult(bool,object)"/>
    public CreateHintResult(object? data) : this(true, data) { }
}
