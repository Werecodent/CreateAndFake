using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class AsyncListTests() : AutoTester(typeof(AsyncList<>))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { InjectionValues = [Tools.Gen.Next(6, 9)] };

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
        AsyncList<DataSample> instance = new(sample, Tools.Valuer.Options.Design.IterationLimit);

        await instance.Assert().IsAsync(sample, canceler);
        await instance.Assert().IsAsync(sample, canceler);
        await instance.Assert().HasCountAsync(sample.Count, canceler);
    }
}
