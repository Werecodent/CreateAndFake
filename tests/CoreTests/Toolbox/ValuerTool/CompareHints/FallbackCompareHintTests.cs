using System.Collections;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class FallbackCompareHintTests : CompareHintTestBase<FallbackCompareHint>
{
    private static readonly FallbackCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public FallbackCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
