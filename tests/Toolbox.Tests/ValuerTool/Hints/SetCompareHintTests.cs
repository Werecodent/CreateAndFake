using System.Collections.Frozen;
using System.Collections.Immutable;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class SetCompareHintTests : CompareHintTestBase<SetCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(ISet<int>),
        typeof(HashSet<string>),
        typeof(HashSet<DataSample>),
        typeof(ISet<FieldSample>),
        typeof(ISet<Type>),
    ];

    public SetCompareHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_CanCheckCollectionType(HashSet<string> data)
    {
        TestInstance
            .TryToCompare(
                data,
                data.ToFrozenSet(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true })
            )
            .Data.Assert()
            .HasCount(1);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_CanCheckCollectionType(ImmutableHashSet<string> data)
    {
        return TestInstance
            .TryToAsyncCompare(
                data,
                data.ToFrozenSet(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true }),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_NoDifferencesWhenEqualWithDifferentTypes(
        ImmutableHashSet<string> data
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data.ToFrozenSet(),
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_FindsDifferencesWithDifferentTypes(
        ImmutableHashSet<int> data,
        FrozenSet<int> data2
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
