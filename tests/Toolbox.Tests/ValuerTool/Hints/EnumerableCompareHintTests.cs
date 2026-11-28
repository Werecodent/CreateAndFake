using System.Collections;
using System.Collections.Immutable;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EnumerableCompareHintTests : CompareHintTestBase<EnumerableCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IList),
        typeof(List<string>),
        typeof(List<KeyValuePair<int, int>>),
    ];

    public EnumerableCompareHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal void Compare_SizeMismatchOutOfBounds(List<string> original)
    {
        IValuerChainer chainer = CreateChainer(
            Tools.Valuer.Options with
            {
                IncludeValueHashInComparison = false,
            }
        );

        List<string> variant = original.Tools().Copy();
        variant.RemoveAt(variant.Count - 1);

        DifferenceHintResult result = TestInstance.TryToCompare(original, variant, chainer);
        result.HasData.Assert().Is(true);
        result.Data.Assert().HasCount(1);
        result.Data.Single().ToString().Assert().Contains("'out of range'");

        result = TestInstance.TryToCompare(variant, original, chainer);
        result.HasData.Assert().Is(true);
        result.Data.Assert().HasCount(1);
        result.Data.Single().ToString().Assert().Contains("'out of range'");
    }

    [Theory, RandomData]
    internal void TryToCompare_CanCheckCollectionType(LinkedList<string> data)
    {
        TestInstance
            .TryToCompare(
                data,
                data.ToImmutableList(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true })
            )
            .Data.Assert()
            .HasCount(1);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_CanCheckCollectionType(LinkedList<string> data)
    {
        return TestInstance
            .TryToAsyncCompare(
                data,
                data.ToImmutableList(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true }),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_NoDifferencesWhenEqualWithDifferentTypes(
        LinkedList<string> data
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data.ToImmutableList(),
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_FindsDifferencesWithDifferentTypes(
        LinkedList<int> data,
        ImmutableList<int> data2
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
