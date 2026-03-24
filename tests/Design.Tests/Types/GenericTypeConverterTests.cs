using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests.Types;

public static class GenericTypeConverterTests
{
    [Fact]
    internal static Task GenericTypeConverter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(GenericTypeConverter),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static Task GenericTypeConverter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(GenericTypeConverter),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static void FindConcreteType_FindsBaseClasses()
    {
        GenericTypeConverter
            .FindConcreteType<List<int>>(typeof(IList<>))
            .Assert()
            .Is(typeof(IList<int>));
        GenericTypeConverter
            .FindConcreteType<ISet<string>>(typeof(IEnumerable<>))
            .Assert()
            .Is(typeof(IEnumerable<string>));
        GenericTypeConverter.FindConcreteType<Task<string>>(typeof(Task<>));
    }

    [Fact]
    internal static void FindConcreteType_ThrowsWhenMissing()
    {
        typeof(List<>)
            .Assert(GenericTypeConverter.FindConcreteType<IList<int>>)
            .Throws<InvalidOperationException>();
        typeof(int)
            .Assert(GenericTypeConverter.FindConcreteType<string>)
            .Throws<InvalidOperationException>();
    }

    [Fact]
    internal static void AsConcreteType_NullWhenMissing()
    {
        GenericTypeConverter.AsConcreteType<IList<int>>(typeof(List<>)).Assert().IsNull();
        GenericTypeConverter.AsConcreteType<string>(typeof(int)).Assert().IsNull();
        GenericTypeConverter.AsConcreteType(null, typeof(IEnumerable<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsConcreteType_ExcludesGenericTypeDefinitions()
    {
        GenericTypeConverter.AsConcreteType(typeof(IList<>), typeof(IList<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsGenericBase_ConvertsGenerics()
    {
        GenericTypeConverter.AsGenericBase(typeof(List<int>)).Assert().Is(typeof(List<>));
    }

    [Fact]
    internal static void AsGenericBase_NullForNonGeneric()
    {
        GenericTypeConverter.AsGenericBase(typeof(string)).Assert().Is(null);
    }

    [Theory, RandomData]
    internal static void ExpandedName_ObjectGetType(IList<int> data)
    {
        GenericTypeConverter.ExpandedName(data).Assert().Contains(nameof(Int32));
    }

    [Fact]
    internal static void ExpandedName_SameWithoutGenerics()
    {
        GenericTypeConverter.ExpandedName<DataSample>().Assert().Is(nameof(DataSample));
    }

    [Fact]
    internal static void ExpandedName_IncludesGenerics()
    {
        GenericTypeConverter
            .ExpandedName<Dictionary<int, string>>()
            .Assert()
            .Contains(nameof(Int32))
            .And.Contains(nameof(String));
    }

    [Fact]
    internal static void BuildTestName_IncludesParameters()
    {
        GenericTypeConverter
            .BuildTestName(
                typeof(GenericTypeConverterTests).GetMethod(
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
