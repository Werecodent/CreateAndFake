using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SubclassCreateHintTests : CreateHintTestBase<SubclassCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IIsGoodOrBadSample)];

    private static readonly Type[] _InvalidTypes = [typeof(VoidType)];

    public SubclassCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
