using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class FakeCreateHintTests : CreateHintTestBase<FakeCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Fake<object>),
        typeof(Fake<OutSample>),
        typeof(Fake<GenericSample<string>>),
    ];

    public FakeCreateHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal static void TryToCreate_VoidCovered([Fake] Holder<string> sample, string data)
    {
        sample.Assert(x => x.WriteValue(data)).ThrowsNoException();
    }

    [Theory, RandomData]
    internal static void TryToCreate_GenericsCovered([Fake] GenericSample<string> sample)
    {
        sample.Run("").Assert().IsNotNull();
    }
}
