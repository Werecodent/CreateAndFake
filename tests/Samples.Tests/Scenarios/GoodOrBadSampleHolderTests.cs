using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public sealed class GoodOrBadSampleHolderTests : AutoTester<GoodOrBadSampleHolder<IsGoodSample>>
{
    [Fact]
    public static void GoodOrBadSampleHolder_CanInject()
    {
        for (int i = 0; i < 10; i++)
        {
            _ = Tools.Randomizer.Inject(typeof(GoodOrBadSampleHolder<>), []);
        }
    }
}
