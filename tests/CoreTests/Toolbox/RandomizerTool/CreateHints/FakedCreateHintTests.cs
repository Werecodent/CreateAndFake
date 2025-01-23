using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class FakedCreateHintTests : CreateHintTestBase<FakedCreateHint>
{
    private static readonly FakedCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IFaked)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public FakedCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
