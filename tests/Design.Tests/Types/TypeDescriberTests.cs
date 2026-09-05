using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Randomization.Handlers;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Design.Tests.Types;

public static class TypeDescriberTests
{
    [Theory, RandomData]
    internal static void Debug_TypeDescriber_ToString(TypeDescriber describer)
    {
        describer.ToString().Assert().Debug();
    }

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
