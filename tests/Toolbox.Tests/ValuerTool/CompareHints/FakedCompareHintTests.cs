using System.Collections;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

public sealed class FakedCompareHintTests : CompareHintTestBase<FakedCompareHint>
{
    private static readonly FakedCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IFaked)];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(string), typeof(int)];

    public FakedCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
