using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <summary>Contains whether or not the <see cref="IMutateHint"/> modified the instance.</summary>
public sealed class MutateHintResult : HintResult<bool>
{
    /// <summary>Result for when a hint doesn't support a <see cref="Type"/>.</summary>
    public static MutateHintResult None { get; } = new(false, default);

    /// <inheritdoc/>
    private MutateHintResult(bool hasData, bool data)
        : base(hasData, data) { }

    /// <inheritdoc cref="MutateHintResult(bool,bool)"/>
    public MutateHintResult(bool data)
        : this(true, data) { }
}
