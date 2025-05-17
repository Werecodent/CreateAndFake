using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class OptionsCreateHintTests : CreateHintTestBase<OptionsCreateHint>
{
    private static readonly OptionsCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(RandomizerOptions)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public OptionsCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
