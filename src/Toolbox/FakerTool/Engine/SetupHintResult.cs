using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.FakerTool.Engine;

/// <inheritdoc/>
public sealed class SetupHintResult : HintResult<bool>
{
    /// <summary>For when a hint doesn't support a type or fails to fake it.</summary>
    public static SetupHintResult None { get; } = new(false, false);

    /// <inheritdoc/>
    private SetupHintResult(bool hasData, bool data)
        : base(hasData, data) { }

    /// <inheritdoc cref="FakeHintResult(bool,IFaked)"/>
    public SetupHintResult(bool data)
        : this(true, data) { }
}
