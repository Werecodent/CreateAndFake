using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests.Types;

public static class TypeHelperTests
{
    [Fact]
    internal static Task TypeHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TypeHelper),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static Task TypeHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TypeHelper),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static void FindConcreteType_FindsBaseClasses()
    {
        TypeHelper.FindConcreteType<List<int>>(typeof(IList<>)).Assert().Is(typeof(IList<int>));
        TypeHelper
            .FindConcreteType<ISet<string>>(typeof(IEnumerable<>))
            .Assert()
            .Is(typeof(IEnumerable<string>));
        TypeHelper.FindConcreteType<Task<string>>(typeof(Task<>));
    }

    [Fact]
    internal static void FindConcreteType_ThrowsWhenMissing()
    {
        typeof(List<>)
            .Assert(TypeHelper.FindConcreteType<IList<int>>)
            .Throws<InvalidOperationException>();
        typeof(int).Assert(TypeHelper.FindConcreteType<string>).Throws<InvalidOperationException>();
    }

    [Fact]
    internal static void AsConcreteType_NullWhenMissing()
    {
        TypeHelper.AsConcreteType<IList<int>>(typeof(List<>)).Assert().IsNull();
        TypeHelper.AsConcreteType<string>(typeof(int)).Assert().IsNull();
        TypeHelper.AsConcreteType(null, typeof(IEnumerable<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsConcreteType_ExcludesGenericTypeDefinitions()
    {
        TypeHelper.AsConcreteType(typeof(IList<>), typeof(IList<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsGenericBase_ConvertsGenerics()
    {
        TypeHelper.AsGenericBase(typeof(List<int>)).Assert().Is(typeof(List<>));
    }

    [Fact]
    internal static void AsGenericBase_NullForNonGeneric()
    {
        TypeHelper.AsGenericBase(typeof(string)).Assert().Is(null);
    }

    [Fact]
    internal static void FindLoadedClassTypes_IncludesOnlyClasses()
    {
        TypeHelper
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
        TypeHelper.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Theory, RandomData]
    internal static void FindLoadedTypes_IgnoresReflectError(
        [Stub] Assembly assembly,
        ReflectionTypeLoadException error
    )
    {
        assembly.GetTypes().SetupCall(Behavior<Type[]>.Throw(error, Times.Once));
        TypeHelper.FindLoadedTypes(assembly).Assert().IsEmpty();
        assembly.Assert().Called();
    }

    [Fact]
    internal static void IsVisible_TrueForPublicClasses()
    {
        TypeHelper.IsVisible<DataSample>(typeof(string).Assembly.GetName()).Assert().Is(true);
    }

    [Fact]
    internal static void IsVisible_TrueForInternalsWithAttribute()
    {
        TypeHelper
            .IsVisible<InternalSample>(Assembly.GetExecutingAssembly().GetName())
            .Assert()
            .Is(true);
    }

    [Fact]
    internal static void IsVisible_FalseForInternalsWithoutAttribute()
    {
        TypeHelper.IsVisible<InternalSample>(typeof(string).Assembly.GetName()).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void ExpandedName_ObjectGetType(IList<int> data)
    {
        TypeHelper.ExpandedName(data).Assert().Contains(nameof(Int32));
    }

    [Fact]
    internal static void ExpandedName_SameWithoutGenerics()
    {
        TypeHelper.ExpandedName<DataSample>().Assert().Is(nameof(DataSample));
    }

    [Fact]
    internal static void ExpandedName_IncludesGenerics()
    {
        TypeHelper
            .ExpandedName<Dictionary<int, string>>()
            .Assert()
            .Contains(nameof(Int32))
            .And.Contains(nameof(String));
    }

    [Fact]
    internal static void BuildTestName_IncludesParameters()
    {
        TypeHelper
            .BuildTestName(
                typeof(TypeHelperTests).GetMethod(
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
