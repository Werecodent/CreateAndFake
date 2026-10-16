using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncListTests
{
    [Fact]
    internal static void Debug_AsyncList_ToString()
    {
        typeof(AsyncList<>).Tools().CreateRandomInstance().ToString().Assert().Debug();
    }

    [Fact]
    internal static Task AsyncList_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AsyncList_NoParameterMutation([Cap(6, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }

    [Fact]
    internal static Task AsyncList_FrameworkSupport()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            [typeof(AsyncList<>)],
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AsyncList_AttributeSupport(
        AsyncList<AsyncDataSample> sample,
        [Copy] AsyncList<AsyncDataSample> clone,
        AsyncList<AsyncDataSample> variant
    )
    {
        return sample
            .Assert()
            .IsAsync(clone, TestContext.Current.CancellationToken)
            .And()
            .IsNotAsync(variant, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task GetAsyncEnumerator_CanCancel()
    {
        return Tools.Tester.VerifySupportsCancellationAsync(
            size => new AsyncList<DataSample>(
                Tools.Randomizer.CreateSized<IEnumerable<DataSample>>(size),
                size
            ),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task GetAsyncEnumerator_Repeatable(IReadOnlyCollection<DataSample> sample)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        AsyncList<DataSample> instance = new(sample, Tools.Valuer.Options.IterationLimit);

        await instance.Assert().IsAsync(sample, canceler);
        await instance.Assert().IsAsync(sample, canceler);
        await instance.Assert().HasCountAsync(sample.Count, canceler);
    }
}
