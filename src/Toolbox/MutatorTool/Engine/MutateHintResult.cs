using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public sealed class MutateHintResult : HintResult<bool>
{
    /// <summary>For when a hint doesn't support a type or fails to mutate it.</summary>
    public static MutateHintResult None { get; } = new(false, default);

    /// <inheritdoc/>
    private MutateHintResult(bool hasData, bool data)
        : base(hasData, data) { }

    /// <inheritdoc cref="MutateHintResult(bool,bool)"/>
    public MutateHintResult(bool data)
        : this(true, data) { }
}
