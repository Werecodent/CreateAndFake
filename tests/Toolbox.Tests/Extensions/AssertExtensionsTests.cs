namespace CreateAndFake.Tests.Extensions;

public static class AssertExtensionsTests
{
    [Fact]
    internal static Task AssertExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(AssertExtensions),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AssertExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(AssertExtensions),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Assert_ObjectIsFluent(int data)
    {
        data.Assert().Is(data).And.IsNot(null);
    }

    [Theory, RandomData]
    internal static void Assert_StringIsFluent(string data)
    {
        data.Assert().Is(data).And.Contains(data).And.HasCount(data.Length);
    }

    [Theory, RandomData]
    internal static void Assert_CollectionIsFluent(ICollection<object> data)
    {
        data.Assert().IsNotEmpty().And.Contains(data.First());
    }

    [Theory, RandomData]
    internal static void Assert_ReadOnlyCollectionIsFluent(IReadOnlyCollection<object> data)
    {
        data.Assert().IsNotEmpty().And.Contains(data.First());
    }

    [Theory, RandomData]
    public static void Assert_ComparableIsFluent(int data)
    {
        data.Assert().GreaterThanOrEqualTo(int.MinValue).And.LessThanOrEqualTo(int.MaxValue);
    }

    [Theory, RandomData]
    public static void Assert_TypeIsFluent(Exception data)
    {
        data.GetType().Assert().Inherits<Exception>().And.Inherits<object>();
    }
}
