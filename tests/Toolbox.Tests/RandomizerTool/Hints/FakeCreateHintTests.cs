using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class FakeCreateHintTests : CreateHintTestBase<FakeCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Fake<object>),
        typeof(Fake<OutSample>),
        typeof(Fake<GenericSample<string>>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public FakeCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
