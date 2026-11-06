using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.FakerTool.Engine;

/// <inheritdoc/>
public sealed class FakeHintResult : HintResult<IFaked?>
{
    /// <summary>For when a hint doesn't support a type or fails to fake it.</summary>
    public static FakeHintResult None { get; } = new(false, null);

    /// <inheritdoc/>
    private FakeHintResult(bool hasData, IFaked? data)
        : base(hasData, data) { }

    /// <inheritdoc cref="FakeHintResult(bool,IFaked)"/>
    public FakeHintResult(IFaked? data)
        : this(true, data) { }
}
