using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public sealed class ValuerAsyncComparableSampleTests : AutoTester<ValuerAsyncComparableSample>
{
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
    public static Task CompareAsync_VariantHasDifferences(
        ValuerAsyncComparableSample data,
        ValuerAsyncComparableSample variant
    )
    {
        return AsyncSeriesHelper
            .ToListAsync(
                data.CompareAsync(variant, Tools.Valuer, TestContext.Current.CancellationToken),
                Tools.Valuer.Options.Design.IterationLimit,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNot(Enumerable.Empty<Difference>());
    }
}
