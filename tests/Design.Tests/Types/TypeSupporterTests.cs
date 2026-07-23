using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class TypeSupporterTests
{
    [Fact]
    internal static Task TypeSupporter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TypeSupporter),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TypeSupporter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
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
        ITypeSupporter item2 = item.Tools().Unique();

        TypeSupporter
            .GroupBySupportedType([item, item2])
            .Assert()
            .Contains(new KeyValuePair<Type, ITypeSupporter>(item.SupportedType, item))
            .And.Contains(new KeyValuePair<Type, ITypeSupporter>(item2.SupportedType, item2));
    }

    [Fact]
    internal static void GroupBySupportedType_IgnoresNullSupportedType()
    {
        TypeSupporter.GroupBySupportedType([TypeDescriber.For(null)]).Assert().IsEmpty();
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
        TypeSupporter.GroupByInheritance([TypeDescriber.For(null)]).Assert().IsEmpty();
    }

    [Theory, RandomData]
    internal static void GroupBySubclasses_JoinsWithSubclass(
        [Stub] ITypeSupporter item1,
        [Stub] ITypeSupporter item2
    )
    {
        item1.SupportedType.SetupReturn(typeof(object));
        item2.SupportedType.SetupReturn(typeof(string));
        TypeSupporter
            .GroupBySubclasses([item1, item2])[typeof(string)]
            .Assert()
            .Contains(item1)
            .And.Contains(item2);
    }

    [Fact]
    internal static void GroupBySubclasses_IgnoresNullSupportedType()
    {
        TypeSupporter.GroupBySubclasses([TypeDescriber.For(null)]).Assert().IsEmpty();
    }
}
