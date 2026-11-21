using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public sealed class ValuerComparableSampleTests : AutoTester<ValuerComparableSample>
{
    [Theory, RandomData]
    public static void Compare_CloneHasNoDifferences(ValuerComparableSample data)
    {
        data.Compare(data.Tools().Copy(), Tools.Valuer).Assert().IsEmpty();
    }

    [Theory, RandomData]
    public static void Compare_UniqueHasDifferences(ValuerComparableSample data)
    {
        data.Compare(data.Tools().Unique(), Tools.Valuer).ToArray().Assert().IsNotEmpty();
    }
}
