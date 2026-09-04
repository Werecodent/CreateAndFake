using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool;

public static class AsyncContentMapTests
{
    [Theory, RandomData]
    internal static void Debug_AsyncContentMap_ToString(AsyncContentMap map)
    {
        map.ToString().Assert().Debug();
    }

    [Fact]
    internal static Task AsyncContentMap_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AsyncContentMap>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncContentMap_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AsyncContentMap>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Extractor.Options],
                    IgnorableExceptions = [typeof(ToolException)],
                }
        );
    }

    [Theory, RandomData]
    internal static async Task HasContentAsync_UsesObjectByValue(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.HasContentAsync(
                sample.NestedValue.Tools().Variant(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
        await map.HasContentAsync(
                sample.NestedValue.Tools().Copy(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HasContentAsync_UsesValueByValue(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.HasContentAsync(
                sample.NestedValue.NumberValue.Tools().Variant(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
        await map.HasContentAsync(
                sample.NestedValue.NumberValue.Tools().Copy(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HasContentAsync_UsesStringByValue(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.HasContentAsync(
                sample.NestedValue.StringValue.Tools().Variant(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
        await map.HasContentAsync(
                sample.NestedValue.StringValue.Tools().Copy(),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task FindAllAsync_ContainsNestedObjects(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.FindAllAsync<DataSample>(TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue, TestContext.Current.CancellationToken);
        await map.FindAllAsync(typeof(DataSample), TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task FindAllAsync_ContainsNestedValues(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.FindAllAsync<int>(TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue.NumberValue, TestContext.Current.CancellationToken);
        await map.FindAllAsync(typeof(int), TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue.NumberValue, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task AllContentAsync_ContainsEverything(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        await map.AllContentAsync(TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue, TestContext.Current.CancellationToken);
        await map.AllContentAsync(TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue.NumberValue, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task FindSharedContentAsync_ObjectValuesFound(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        IAsyncContentMap test = await Tools.Extractor.ExtractAsync(
            sample.NestedValue,
            TestContext.Current.CancellationToken
        );
        await map.FindSharedContentAsync(test, TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue, TestContext.Current.CancellationToken);
        await map.HasSharedContentAsync(test, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task FindSharedContentAsync_ValueValuesFound(DataHolderSample sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        IAsyncContentMap test = await Tools.Extractor.ExtractAsync(
            sample.NestedValue.NumberValue,
            TestContext.Current.CancellationToken
        );
        await map.FindSharedContentAsync(test, TestContext.Current.CancellationToken)
            .Assert()
            .ContainsAsync(sample.NestedValue.NumberValue, TestContext.Current.CancellationToken);
        await map.HasSharedContentAsync(test, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HasSharedContentAsync_FalseWithNothingShared(string sample)
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );
        IAsyncContentMap test = await Tools.Extractor.ExtractAsync(
            sample.Tools().Variant(),
            TestContext.Current.CancellationToken
        );
        await map.HasSharedContentAsync(test, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HasSharedContentAsync_IgnoresSimpleTypes(
        char sample1,
        bool sample2,
        BindingFlags sample3
    )
    {
        IAsyncContentMap map1 = await Tools.Extractor.ExtractAsync(
            sample1,
            TestContext.Current.CancellationToken
        );
        await map1.HasSharedContentAsync(
                await Tools.Extractor.ExtractAsync(
                    sample1.Tools().Copy(),
                    TestContext.Current.CancellationToken
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);

        IAsyncContentMap map2 = await Tools.Extractor.ExtractAsync(
            sample2,
            TestContext.Current.CancellationToken
        );
        await map2.HasSharedContentAsync(
                await Tools.Extractor.ExtractAsync(
                    sample2.Tools().Copy(),
                    TestContext.Current.CancellationToken
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);

        IAsyncContentMap map3 = await Tools.Extractor.ExtractAsync(
            sample3,
            TestContext.Current.CancellationToken
        );
        await map3.HasSharedContentAsync(
                await Tools.Extractor.ExtractAsync(
                    sample3.Tools().Copy(),
                    TestContext.Current.CancellationToken
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(false, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task FindSharedContentAsync_EmptyCollectionsIgnored(
        [Size(0)] object[] sample
    )
    {
        IAsyncContentMap map = await Tools.Extractor.ExtractAsync(
            sample,
            TestContext.Current.CancellationToken
        );

        await map.FindSharedContentAsync(
                await Tools.Extractor.ExtractAsync(
                    Array.Empty<object>(),
                    TestContext.Current.CancellationToken
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .IsEmptyAsync(TestContext.Current.CancellationToken);
    }
}
