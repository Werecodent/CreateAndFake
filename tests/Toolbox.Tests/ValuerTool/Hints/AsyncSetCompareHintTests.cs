using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AsyncSetCompareHintTests : CompareHintTestBase<AsyncSetCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IAsyncSet<int>),
        typeof(IAsyncSet<string>),
        typeof(IAsyncSet<object>),
        typeof(AsyncHashSet<int>),
        typeof(AsyncHashSet<string>),
        typeof(AsyncHashSet<object>),
    ];

    protected override bool RequiresAsync { get; } = true;

    public AsyncSetCompareHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_RequiresSameItemType(HashSet<string> data, IAsyncSet<int> data2)
    {
        return TestInstance
            .TryToAsyncCompare(
                data,
                data2,
                CreateChainer(
                    Tools.Valuer.Options with
                    {
                        CheckCollectionType = true,
                        IncludeValueHashInComparison = false,
                    }
                ),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_NoDifferencesWhenEqualWithDifferentTypes(HashSet<string> data)
    {
        IAsyncSet<string> data2 = AsyncHashSet.CreateFromAsync(
            data,
            Tools.Valuer.ToAsyncComparer<string>(),
            data.Count,
            TestContext.Current.CancellationToken
        );

        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data2,
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_FindsDifferencesWithDifferentTypes(
        AsyncHashSet<string> data,
        HashSet<string> data2
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data,
            data2,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountMoreThanAsync(0, TestContext.Current.CancellationToken);
    }
}
