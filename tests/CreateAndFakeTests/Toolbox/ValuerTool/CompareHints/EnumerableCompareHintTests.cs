using System.Collections;
using CreateAndFake.Toolbox.ValuerTool;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class EnumerableCompareHintTests : CompareHintTestBase<EnumerableCompareHint>
{
    private static readonly EnumerableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IList),
        typeof(List<string>),
        typeof(List<KeyValuePair<int, int>>)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(int)
    ];

    public EnumerableCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_SizeMismatchOutOfBounds(List<string> original)
    {
        List<string> variant = original.CreateDeepClone();
        variant.RemoveAt(variant.Count - 1);

        DifferenceHintResult result = TestInstance.TryCompare(original, variant, CreateChainer());
        result.HasData.Assert().Is(true);
        result.Data.Assert().HasCount(1);
        result.Data.Single().ToString().Assert().Contains("outofbounds");

        result = TestInstance.TryCompare(variant, original, CreateChainer());
        result.HasData.Assert().Is(true);
        result.Data.Assert().HasCount(1);
        result.Data.Single().ToString().Assert().Contains("outofbounds");
    }
}
