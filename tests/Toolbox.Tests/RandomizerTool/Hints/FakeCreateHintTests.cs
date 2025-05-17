using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class FakeCreateHintTests : CreateHintTestBase<FakeCreateHint>
{
    private static readonly FakeCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Fake<object>),
        typeof(Fake<OutSample>),
        typeof(Fake<GenericSample<string>>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public FakeCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
