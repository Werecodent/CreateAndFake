using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class DictionaryCompareHintTests : CompareHintTestBase<DictionaryCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(IDictionary),
        typeof(Dictionary<string, int>),
        typeof(IDictionary<Type, string>),
    ];

    public DictionaryCompareHintTests()
        : base(_ValidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_SameKeyDifferentValuesWorks(Dictionary<string, int> data)
    {
        Dictionary<string, int> dupe = data.Tools().Copy();
        string key = data.First().Key;
        dupe[key] = data[key].Tools().Variant();

        TestInstance
            .TryToCompare(data, dupe, CreateChainer())
            .Data.ToArray()
            .Assert()
            .IsNotEmpty(
                "Hint didn't find differences with a modified key on '"
                    + GenericConverter.ExpandName(data)
                    + "'."
            );
    }

    [Theory, RandomData]
    internal void TryToCompare_CanCheckCollectionType(ConcurrentDictionary<Type, string> data)
    {
        TestInstance
            .TryToCompare(
                data,
                data.ToFrozenDictionary(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true })
            )
            .Data.Assert()
            .HasCount(1);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_CanCheckCollectionType(ConcurrentDictionary<Type, string> data)
    {
        return TestInstance
            .TryToAsyncCompare(
                data,
                data.ToFrozenDictionary(),
                CreateChainer(Tools.Valuer.Options with { CheckCollectionType = true }),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_NoDifferencesWhenEqualWithDifferentTypes(
        ConcurrentDictionary<Type, string> data
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data.ToFrozenDictionary(),
            data,
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryToAsyncCompare_FindsDifferencesWithDifferentTypes(
        ConcurrentDictionary<int, string> data,
        FrozenDictionary<int, string> data2
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

    [Theory, RandomData]
    internal Task TryToAsyncCompare_DifferencesWithSameKeyButNotValues(
        IDictionary<int, string> data
    )
    {
        DifferenceHintAsyncResult result = TestInstance.TryToAsyncCompare(
            data,
            data.ToDictionary(p => p.Key, p => p.Value.Tools().Variant()),
            CreateChainer(),
            TestContext.Current.CancellationToken
        );

        result.HasData.Assert().Is(true);
        return result.Data.Assert().HasCountMoreThanAsync(0, TestContext.Current.CancellationToken);
    }
}
