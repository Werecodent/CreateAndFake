using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Randomization.Handlers;
using CreateAndFake.Design.Types;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Design.Tests.Types;

public static class TypeDescriberTests
{
    [Fact]
    internal static Task TypeDescriber_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<TypeDescriber>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TypeDescriber_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<TypeDescriber>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void For_Cached()
    {
        TypeDescriber.For<string>().Assert().ReferenceEqual(TypeDescriber.For<string>());
    }

    [Fact]
    internal static void For_NullCachedAsEmpty()
    {
        TypeDescriber nullTracker = TypeDescriber.For(null);
        TypeDescriber.For(null).Assert().ReferenceEqual(nullTracker);
        nullTracker.InheritedTypes.Assert().IsEmpty();
    }

    [Fact]
    internal static void Inherits_IncludesGenerics()
    {
        TypeDescriber collection = TypeDescriber.For<List<int>>();
        collection.Inherits<List<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<int>>().Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_IncludesGenericBases()
    {
        TypeDescriber collection = TypeDescriber.For(typeof(IList<>));
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<List<int>>().Assert().Is(false);
        collection.Inherits(typeof(List<>)).Assert().Is(false);
        collection.Inherits(typeof(IEnumerable<>)).Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_SelfIncluded()
    {
        TypeDescriber.For<string>().Inherits<string>().Assert().Is(true);
    }

    [Fact]
    internal static void FindLocalSubclasses_ExcludesOtherAssemblies()
    {
        TypeDescriber
            .For<IValuerEquatable>()
            .FindLocalSubclasses()
            .Assert()
            .ContainsNot(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLocalSubclasses_SelfIncluded()
    {
        TypeDescriber
            .For<ValueComparer>()
            .FindLocalSubclasses()
            .Assert()
            .Contains(typeof(ValueComparer));
    }

    [Fact]
    internal static void FindLoadedSubclasses_IncludesFromDifferentAssemblies()
    {
        TypeDescriber
            .For<IValuerEquatable>()
            .FindLoadedSubclasses()
            .Assert()
            .Contains(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLoadedSubclasses_SelfIncluded()
    {
        TypeDescriber
            .For<ValueComparer>()
            .FindLoadedSubclasses()
            .Assert()
            .Contains(typeof(ValueComparer));
    }

    [Fact]
    internal static void FindLoadedSubclasses_ExcludesExclusionAttribute()
    {
        TypeDescriber
            .For<ITypeSupporter>()
            .FindLoadedSubclasses()
            .Assert()
            .ContainsNot(typeof(RuneValueHandler));
    }
}
