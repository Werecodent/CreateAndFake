using System.Collections;
using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

public sealed class EarlyFailCompareHintTests : CompareHintTestBase<EarlyFailCompareHint>
{
    private static readonly EarlyFailCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(int),
        typeof(string),
        typeof(BindingFlags),
        typeof(Type),
        typeof(Delegate),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IDictionary),
        typeof(IEnumerable),
        typeof(IAsyncEnumerable<int>),
    ];

    public EarlyFailCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    internal void TryCompare_NullBehaviorCheck()
    {
        TestInstance.TryCompare(null, new object(), CreateChainer()).HasData.Assert().Is(true);
        TestInstance.TryCompare(null, new object(), CreateChainer()).Data.Assert().IsNotEmpty();

        TestInstance.TryCompare(null, null, CreateChainer()).HasData.Assert().Is(true);
        TestInstance.TryCompare(null, null, CreateChainer()).Data.Assert().IsEmpty();

        TestInstance.TryCompare(new object(), null, CreateChainer()).HasData.Assert().Is(true);
        TestInstance.TryCompare(new object(), null, CreateChainer()).Data.Assert().IsNotEmpty();
    }

    [Fact]
    internal void TryGetHashCode_NullBehaviorCheck()
    {
        TestInstance
            .TryGetHashCode(null, CreateChainer())
            .Assert()
            .Is(new HashCodeHintResult(ValueComparer.NullHash));
    }

    [Theory, RandomData]
    internal void TryCompare_MismatchedTypesDifferent(int value)
    {
        TestInstance.TryCompare(value, new object(), CreateChainer()).Data.Assert().IsNotEmpty();
    }
}
