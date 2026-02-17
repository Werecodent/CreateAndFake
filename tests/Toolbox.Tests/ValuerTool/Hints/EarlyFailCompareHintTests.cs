using System.Collections;
using System.Reflection;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EarlyFailCompareHintTests : CompareHintTestBase<EarlyFailCompareHint>
{
    private static readonly EarlyFailCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(int),
        typeof(BindingFlags),
        typeof(Delegate),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IDictionary),
        typeof(IAsyncEnumerable<int>),
    ];

    public EarlyFailCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCompare_MismatchedTypesDifferent(int value)
    {
        TestInstance.TryCompare(value, new object(), CreateChainer()).Data.Assert().IsNotEmpty();
    }
}
