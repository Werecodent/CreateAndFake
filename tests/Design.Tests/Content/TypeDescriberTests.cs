using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Design.Tests.Content;

public static class TypeDescriberTests
{
    [Fact]
    internal static Task TypeDescriber_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(TypeDescriber),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static Task TypeDescriber_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(TypeDescriber),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static void FindConcreteInterface_FindsBaseClasses()
    {
        TypeDescriber
            .FindConcreteInterface<List<int>>(typeof(IList<>))
            .Assert()
            .Is(typeof(IList<int>));
        TypeDescriber
            .FindConcreteInterface<ISet<string>>(typeof(IEnumerable<>))
            .Assert()
            .Is(typeof(IEnumerable<string>));
    }

    [Fact]
    internal static void FindConcreteInterface_ThrowsWhenMissing()
    {
        typeof(List<>)
            .Assert(TypeDescriber.FindConcreteInterface<IList<int>>)
            .Throws<InvalidOperationException>();
        typeof(int)
            .Assert(TypeDescriber.FindConcreteInterface<string>)
            .Throws<InvalidOperationException>();
    }

    [Fact]
    internal static void AsGenericBase_ConvertsGenerics()
    {
        TypeDescriber.AsGenericBase(typeof(List<int>)).Assert().Is(typeof(List<>));
    }

    [Fact]
    internal static void AsGenericBase_NullForNonGeneric()
    {
        TypeDescriber.AsGenericBase(typeof(string)).Assert().Is(null);
    }

    [Fact]
    internal static void GetAllFields_FindsPublicFields()
    {
        TypeDescriber.GetAllFields<FieldSample>(true).Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetAllFields_FindsInheritedPrivates()
    {
        TypeDescriber.GetAllFields<InheritedPrivatesSample>(true).Assert().IsEmpty();
        TypeDescriber.GetAllFields<InheritedPrivatesSample>().Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetAllProperties_FindsPublicProperties()
    {
        TypeDescriber.GetAllProperties<DataSample>(true).Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetAllProperties_FindsInheritedPrivates()
    {
        TypeDescriber.GetAllProperties<InheritedPrivatesSample>(true).Assert().IsEmpty();
        TypeDescriber.GetAllProperties<InheritedPrivatesSample>().Assert().IsNotEmpty();
    }

    [Fact]
    internal static void FindLocalSubclasses_ExcludesOtherAssemblies()
    {
        TypeDescriber
            .FindLocalSubclasses<IValuerEquatable>()
            .Assert()
            .ContainsNot(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLoadedSubclasses_IncludesFromDifferentAssemblies()
    {
        TypeDescriber
            .FindLoadedSubclasses<IValuerEquatable>()
            .Assert()
            .Contains(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLoadedClassTypes_IncludesOnlyClasses()
    {
        TypeDescriber
            .FindLoadedClassTypes(typeof(DataSample).Assembly)
            .Assert()
            .Contains(typeof(DataSample))
            .And.ContainsNot(typeof(IIsGoodOrBadSample));
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresMissingAssembly(
        [Stub] Assembly assembly,
        FileNotFoundException error
    )
    {
        assembly.GetTypes().SetupCall(Behavior<Type[]>.Throw(error, Times.Once));
        TypeDescriber.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresReflectError(
        [Stub] Assembly assembly,
        ReflectionTypeLoadException error
    )
    {
        assembly.GetTypes().SetupCall(Behavior<Type[]>.Throw(error, Times.Once));
        TypeDescriber.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Fact]
    internal static void IsVisible_TrueForPublicClasses()
    {
        TypeDescriber.IsVisible<DataSample>(typeof(string).Assembly.GetName()).Assert().Is(true);
    }

    [Fact]
    internal static void IsVisible_TrueForInternalsWithAttribute()
    {
        TypeDescriber
            .IsVisible<InternalSample>(Assembly.GetExecutingAssembly().GetName())
            .Assert()
            .Is(true);
    }

    [Fact]
    internal static void IsVisible_FalseForInternalsWithoutAttribute()
    {
        TypeDescriber
            .IsVisible<InternalSample>(typeof(string).Assembly.GetName())
            .Assert()
            .Is(false);
    }

    [Fact]
    internal static void ExpandedName_SameWithoutGenerics()
    {
        TypeDescriber.ExpandedName(typeof(DataSample)).Assert().Is(nameof(DataSample));
    }

    [Fact]
    internal static void ExpandedName_IncludesGenerics()
    {
        TypeDescriber
            .ExpandedName(typeof(Dictionary<int, string>))
            .Assert()
            .Contains(nameof(Int32))
            .And.Contains(nameof(String));
    }

    [Fact]
    internal static void BuildTestName_IncludesParameters()
    {
        TypeDescriber
            .BuildTestName(
                typeof(TypeDescriberTests).GetMethod(
                    nameof(HiddenTestName),
                    BindingFlags.Static | BindingFlags.NonPublic
                )
            )
            .Assert()
            .Contains(nameof(HiddenTestName))
            .And.Contains(nameof(String))
            .And.Contains(nameof(Int32));
    }

    private static void HiddenTestName(string value, int num)
    {
        ArgumentGuard.ThrowIfNull(value, num);
    }
}
