using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class ValuerAsyncComparableSampleTests
{
    [Fact]
    public static Task ValuerAsyncComparableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerAsyncComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValuerAsyncComparableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerAsyncComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    public static Task CompareAsync_CloneHasNoDifferences(ValuerAsyncComparableSample data)
    {
        return data.CompareAsync(
                data.Tools().Copy(),
                Tools.Valuer,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsEmptyAsync(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    public static Task CompareAsync_VariantHasDifferences(ValuerAsyncComparableSample data)
    {
        return data.CompareAsync(
                data.Tools().Variant(),
                Tools.Valuer,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsNotEmptyAsync(TestContext.Current.CancellationToken);
    }
}
