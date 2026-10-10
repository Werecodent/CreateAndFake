using System.Collections;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class ValueTaskCompareHintTests : CompareHintTestBase<ValueTaskCompareHint>
{
    private static readonly ValueTaskCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(ValueTask<DataHolderSample>),
        typeof(ValueTask<string>),
        typeof(ValueTask<int>),
        typeof(ValueTask<bool>),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public ValueTaskCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
