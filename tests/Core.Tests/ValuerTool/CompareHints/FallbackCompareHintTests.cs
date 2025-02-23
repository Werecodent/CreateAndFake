using System.Collections;
using System.Collections.Frozen;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

public sealed class FallbackCompareHintTests : CompareHintTestBase<FallbackCompareHint>
{
    private static readonly FallbackCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public FallbackCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void Throws_OptionsOkay(string value)
    {
        _TestInstance.TryCompare(value, value,
            CreateChainer(Tools.Valuer.Options with { FallbackTypes = FrozenSet.ToFrozenSet([typeof(string)]) }));
    }
}
