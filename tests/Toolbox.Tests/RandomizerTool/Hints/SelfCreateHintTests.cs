using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SelfCreateHintTests : CreateHintTestBase<SelfCreateHint>
{
    private static readonly SelfCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IRandom),
        typeof(ValueRandom),
        typeof(Limiter),
        typeof(ToolSet),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public SelfCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
