using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SubclassCreateHintTests : CreateHintTestBase<SubclassCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IIsGoodOrBadSample)];

    private static readonly Type[] _InvalidTypes = [typeof(VoidType)];

    public SubclassCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
