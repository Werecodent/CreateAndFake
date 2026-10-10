using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ValuerComparableSampleTests
{
    [Fact]
    public static Task ValuerComparableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValuerComparableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

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
