using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class InheritanceTrackerTests
{
    [Fact]
    internal static Task InheritanceTracker_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InheritanceTracker>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task InheritanceTracker_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InheritanceTracker>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void For_Cached()
    {
        InheritanceTracker.For<string>().Assert().ReferenceEqual(InheritanceTracker.For<string>());
    }

    [Fact]
    internal static void For_NullCachedAsEmpty()
    {
        InheritanceTracker nullTracker = InheritanceTracker.For(null);
        InheritanceTracker.For(null).Assert().ReferenceEqual(nullTracker);
        nullTracker.InheritedTypes.Assert().IsEmpty();
    }

    [Fact]
    internal static void Inherits_IncludesGenerics()
    {
        InheritanceTracker collection = InheritanceTracker.For<List<int>>();
        collection.Inherits<List<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<int>>().Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_IncludesGenericBases()
    {
        InheritanceTracker collection = InheritanceTracker.For(typeof(IList<>));
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<List<int>>().Assert().Is(false);
        collection.Inherits(typeof(List<>)).Assert().Is(false);
        collection.Inherits(typeof(IEnumerable<>)).Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_SelfIncluded()
    {
        InheritanceTracker.For<string>().Inherits<string>().Assert().Is(true);
    }
}
