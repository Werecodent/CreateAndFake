using System.Collections;
using System.Reflection;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ParameterInfoCompareHintTests : CompareHintTestBase<ParameterInfoCompareHint>
{
    private static readonly ParameterInfoCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(ParameterInfo)];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public ParameterInfoCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
