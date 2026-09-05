using System.Collections;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class FakedCompareHintTests : CompareHintTestBase<FakedCompareHint>
{
    private static readonly FakedCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(IFaked)];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public FakedCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
