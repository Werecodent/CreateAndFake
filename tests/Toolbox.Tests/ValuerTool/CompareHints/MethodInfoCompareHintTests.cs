using System.Reflection;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

public sealed class MemberInfoCompareHintTests : CompareHintTestBase<MemberInfoCompareHint>
{
    private static readonly MemberInfoCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(MemberInfo),
        typeof(MethodInfo),
        typeof(MethodBase),
        typeof(ConstructorInfo),
        typeof(ConstructorInfo),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object), typeof(int)];

    public MemberInfoCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
