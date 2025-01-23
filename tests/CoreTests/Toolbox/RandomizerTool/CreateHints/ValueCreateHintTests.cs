using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class ValueCreateHintTests : CreateHintTestBase<ValueCreateHint>
{
    private static readonly ValueCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [.. ValueRandom.ValueTypes];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public ValueCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
