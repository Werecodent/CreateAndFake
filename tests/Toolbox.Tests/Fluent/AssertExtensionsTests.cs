namespace Werecodent.CreateAndFake.Tests.Fluent;

public sealed class AssertExtensionsTests() : AutoTester(typeof(AssertExtensions))
{
    [Theory, RandomData]
    internal static void Assert_ObjectIsFluent(int data)
    {
        data.Assert().Is(data).And().IsNotNull();
    }

    [Theory, RandomData]
    internal static void Assert_StringIsFluent(string data)
    {
        data.Assert().Is(data).And().Contains(data).And().HasCount(data.Length);
    }

    [Theory, RandomData]
    internal static void Assert_CollectionIsFluent(ICollection<object> data)
    {
        data.Assert().IsNotEmpty().And().Contains(data.First());
    }

    [Theory, RandomData]
    internal static void Assert_ReadOnlyCollectionIsFluent(IReadOnlyCollection<object> data)
    {
        data.Assert().IsNotEmpty().And().Contains(data.First());
    }

    [Theory, RandomData]
    public static void Assert_ComparableIsFluent(int data)
    {
        data.Assert().GreaterThanOrEqualTo(int.MinValue).And().LessThanOrEqualTo(int.MaxValue);
    }
}
