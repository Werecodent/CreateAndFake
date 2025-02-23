using CreateAndFake.RandomizerTool.CreateHints;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class GenericCreateHintTests : CreateHintTestBase<GenericCreateHint>
{
    private static readonly GenericCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IList<>),
        typeof(KeyValuePair<,>),
        typeof(GenericSample<>),
        typeof(ConstraintSample<,>)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(IList<string>),
        typeof(KeyValuePair<int, int>)
    ];

    public GenericCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
