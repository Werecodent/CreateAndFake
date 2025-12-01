using System.Collections;
using System.Reflection;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class MethodBaseCompareHintTests : CompareHintTestBase<MethodBaseCompareHint>
{
    private static readonly MethodBaseCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(MemberInfo),
        typeof(MethodBase),
        typeof(ConstructorInfo),
        typeof(MethodInfo),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public MethodBaseCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
