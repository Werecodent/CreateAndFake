using System.Collections;
using System.Reflection;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class DefaultEqualityCompareHintTests
    : CompareHintTestBase<DefaultEqualityCompareHint>
{
    private static readonly DefaultEqualityCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(BindingFlags), typeof(Func<>)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(IEnumerable<int>)];

    public DefaultEqualityCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_MismatchedTypesDifferent(int value)
    {
        TestInstance.TryToCompare(value, new object(), CreateChainer()).Data.Assert().IsNotEmpty();
    }
}
