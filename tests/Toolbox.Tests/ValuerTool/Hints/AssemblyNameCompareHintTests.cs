using System.Collections;
using System.Reflection;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class AssemblyNameCompareHintTests : CompareHintTestBase<AssemblyNameCompareHint>
{
    private static readonly AssemblyNameCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(AssemblyName)];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public AssemblyNameCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
