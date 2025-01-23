using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class FakeCreateHintTests : CreateHintTestBase<FakeCreateHint>
{
    private static readonly FakeCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Fake<object>),
        typeof(Fake<OutSample>),
        typeof(Fake<GenericSample<string>>)
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public FakeCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
