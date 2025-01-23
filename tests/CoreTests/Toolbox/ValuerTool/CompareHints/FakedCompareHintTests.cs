using System.Collections;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class FakedCompareHintTests : CompareHintTestBase<FakedCompareHint>
{
    private static readonly FakedCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IFaked)];

    private static readonly Type[] _InvalidTypes = [typeof(IEnumerable), typeof(string), typeof(int)];

    public FakedCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
