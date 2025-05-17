using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class FakedCreateHintTests : CreateHintTestBase<FakedCreateHint>
{
    private static readonly FakedCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IFaked)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public FakedCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
