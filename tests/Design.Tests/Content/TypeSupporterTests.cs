using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class TypeSupporterTests
{
    [Fact]
    internal static Task TypeSupporter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(TypeSupporter),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TypeSupporter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(TypeSupporter),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException), // Duplicate key.
                    ],
                }
        );
    }

    [Theory, RandomData]
    internal static void GroupBySupportedType_GroupsSuccessfully(ITypeSupporter item)
    {
        ITypeSupporter item2 = item.CreateUnique();
        TypeSupporter
            .GroupBySupportedType([item, item2])
            .Assert()
            .Contains(KeyValuePair.Create(item.SupportedType, item))
            .And.Contains(KeyValuePair.Create(item2.SupportedType, item2));
    }

    [Fact]
    internal static void GroupBySupportedType_IgnoresNullSupportedType()
    {
        TypeSupporter.GroupBySupportedType([InheritanceTracker.For(null)]).Assert().IsEmpty();
    }

    [Theory, RandomData]
    internal static void GroupByInheritance_JoinsWithInherited(
        [Stub] ITypeSupporter item1,
        [Stub] ITypeSupporter item2
    )
    {
        item1.SupportedType.SetupReturn(typeof(string));
        item2.SupportedType.SetupReturn(typeof(object));
        TypeSupporter
            .GroupByInheritance([item1, item2])[typeof(object)]
            .Assert()
            .Contains(item1)
            .And.Contains(item2);
    }

    [Fact]
    internal static void GroupByInheritance_IgnoresNullSupportedType()
    {
        TypeSupporter.GroupByInheritance([InheritanceTracker.For(null)]).Assert().IsEmpty();
    }
}
