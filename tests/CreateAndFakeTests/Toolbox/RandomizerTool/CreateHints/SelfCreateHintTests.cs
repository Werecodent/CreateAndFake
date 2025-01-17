using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class SelfCreateHintTests : CreateHintTestBase<SelfCreateHint>
{
    private static readonly SelfCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IRandom), typeof(Limiter), typeof(ToolSet)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public SelfCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}