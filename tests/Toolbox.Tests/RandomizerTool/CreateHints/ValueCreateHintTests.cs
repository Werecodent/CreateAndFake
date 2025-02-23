using CreateAndFake.Design.Randomization;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class ValueCreateHintTests : CreateHintTestBase<ValueCreateHint>
{
    private static readonly ValueCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [.. ValueRandom.ValueTypes];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public ValueCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
