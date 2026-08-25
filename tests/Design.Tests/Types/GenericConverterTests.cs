using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Design.Tests.Types;

public static class GenericConverterTests
{
    private static readonly MethodInfo _TestMethod = typeof(GenericConverterTests).GetMethod(
        nameof(HiddenTestName),
        BindingFlags.Static | BindingFlags.NonPublic
    );

    [Fact]
    internal static void Debug_GenericConverter_BuildTestName()
    {
        GenericConverter.BuildTestName(_TestMethod).Assert().Debug();
    }

    [Fact]
    internal static void Debug_GenericConverter_ExpandName()
    {
        GenericConverter.ExpandName<Dictionary<int, string>>().Assert().Debug();
    }

    [Fact]
    internal static Task GenericConverter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(GenericConverter),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static Task GenericConverter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(GenericConverter),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Fact]
    internal static void FindConcreteType_FindsBaseClasses()
    {
        GenericConverter
            .FindConcreteType<List<int>>(typeof(IList<>))
            .Assert()
            .Is(typeof(IList<int>));
        GenericConverter
            .FindConcreteType<ISet<string>>(typeof(IEnumerable<>))
            .Assert()
            .Is(typeof(IEnumerable<string>));
        GenericConverter.FindConcreteType<Task<string>>(typeof(Task<>));
    }

    [Fact]
    internal static void FindConcreteType_ThrowsWhenMissing()
    {
        typeof(List<>)
            .Assert(GenericConverter.FindConcreteType<IList<int>>)
            .Throws<InvalidOperationException>();
        typeof(int)
            .Assert(GenericConverter.FindConcreteType<string>)
            .Throws<InvalidOperationException>();
    }

    [Fact]
    internal static void AsConcreteType_NullWhenMissing()
    {
        GenericConverter.AsConcreteType<IList<int>>(typeof(List<>)).Assert().IsNull();
        GenericConverter.AsConcreteType<string>(typeof(int)).Assert().IsNull();
        GenericConverter.AsConcreteType(null, typeof(IEnumerable<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsConcreteType_ExcludesGenericTypeDefinitions()
    {
        GenericConverter.AsConcreteType(typeof(IList<>), typeof(IList<>)).Assert().IsNull();
    }

    [Fact]
    internal static void AsGenericBase_ConvertsGenerics()
    {
        GenericConverter.AsGenericBase(typeof(List<int>)).Assert().Is(typeof(List<>));
    }

    [Fact]
    internal static void AsGenericBase_NullForNonGeneric()
    {
        GenericConverter.AsGenericBase(typeof(string)).Assert().IsNull();
    }

    [Fact]
    internal static void AsMatchedGeneric_FindsConcreteType()
    {
        GenericConverter
            .AsMatchedGeneric(typeof(IEnumerable<>), typeof(List<int>))
            .Assert()
            .Is(typeof(IEnumerable<int>));
    }

    [Fact]
    internal static void AsMatchedGeneric_NullWithoutBaseDefinition()
    {
        GenericConverter
            .AsMatchedGeneric(typeof(IEnumerable<string>), typeof(List<int>))
            .Assert()
            .IsNull();
    }

    [Theory, RandomData]
    internal static void ExpandName_ObjectGetType(IList<int> data)
    {
        GenericConverter.ExpandName(data).Assert().Contains(nameof(Int32));
    }

    [Fact]
    internal static void ExpandName_SameWithoutGenerics()
    {
        GenericConverter.ExpandName<DataSample>().Assert().Is(nameof(DataSample));
    }

    [Fact]
    internal static void ExpandName_IncludesGenerics()
    {
        GenericConverter
            .ExpandName<Dictionary<int, string>>()
            .Assert()
            .Contains(nameof(Int32))
            .And.Contains(nameof(String));
    }

    [Fact]
    internal static void BuildTestName_IncludesParameters()
    {
        GenericConverter
            .BuildTestName(_TestMethod)
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
