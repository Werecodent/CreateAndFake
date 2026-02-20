using System.Collections;
using System.Reflection;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class DefaultEqualityCompareHintTests
    : CompareHintTestBase<DefaultEqualityCompareHint>
{
    private static readonly DefaultEqualityCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(BindingFlags), typeof(Delegate)];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IDictionary),
        typeof(IAsyncEnumerable<int>),
    ];

    public DefaultEqualityCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCompare_MismatchedTypesDifferent(int value)
    {
        TestInstance.TryCompare(value, new object(), CreateChainer()).Data.Assert().IsNotEmpty();
    }
}
