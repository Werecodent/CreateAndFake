using System.Collections;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EnumerableCompareHintTests : CompareHintTestBase<EnumerableCompareHint>
{
    private static readonly EnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IList),
        typeof(List<string>),
        typeof(List<KeyValuePair<int, int>>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample), typeof(int)];

    public EnumerableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_SizeMismatchOutOfBounds(List<string> original)
    {
        IValuerChainer chainer = CreateChainer(
            Tools.Valuer.Options with
            {
                IncludeValueHashInComparison = false,
            }
        );

        List<string> variant = original.CreateDeepClone();
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
}
