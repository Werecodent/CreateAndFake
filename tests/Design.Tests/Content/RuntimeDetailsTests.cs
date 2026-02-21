using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class RuntimeDetailsTests
{
    [Fact]
    internal static Task RuntimeDetails_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(RuntimeDetails),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RuntimeDetails_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(RuntimeDetails),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void RuntimeType_CorrectType()
    {
        RuntimeDetails.RuntimeType.FullName.Assert().Is("System.RuntimeType");
    }

    [Fact]
    internal static void RuntimeConstructorInfoType_CorrectType()
    {
        RuntimeDetails
            .RuntimeConstructorInfoType.FullName.Assert()
            .Is("System.Reflection.RuntimeConstructorInfo");
    }

    [Fact]
    internal static void RuntimeMethodInfoType_CorrectType()
    {
        RuntimeDetails
            .RuntimeMethodInfoType.FullName.Assert()
            .Is("System.Reflection.RuntimeMethodInfo");
    }

    [Fact]
    internal static void RuntimePropertyInfoType_CorrectType()
    {
        RuntimeDetails
            .RuntimePropertyInfoType.FullName.Assert()
            .Is("System.Reflection.RuntimePropertyInfo");
    }

    [Fact]
    internal static void RtFieldInfo_CorrectType()
    {
        RuntimeDetails.RtFieldInfoType.FullName.Assert().Is("System.Reflection.RtFieldInfo");
    }

    [Fact]
    internal static void MdFieldInfoType_CorrectType()
    {
        RuntimeDetails.MdFieldInfoType.FullName.Assert().Is("System.Reflection.MdFieldInfo");
    }

    [Fact]
    internal static void RuntimeParameterInfoType_CorrectType()
    {
        RuntimeDetails
            .RuntimeParameterInfoType.FullName.Assert()
            .Is("System.Reflection.RuntimeParameterInfo");
    }

    [Fact]
    internal static void RuntimeAssemblyType_CorrectType()
    {
        RuntimeDetails
            .RuntimeAssemblyType.FullName.Assert()
            .Is("System.Reflection.RuntimeAssembly");
    }

    [Fact]
    internal static void RuntimeTypes_HasAll()
    {
        RuntimeDetails
            .RuntimeTypes.Distinct()
            .Count()
            .Assert()
            .Is(typeof(RuntimeDetails).GetProperties().Length - 1);
    }

    [Theory, RandomData]
    internal static void InnerClassValidation_Functions(string x, string y)
    {
        RuntimeDetails.InnerClassValidation(x, y).Assert().Is(false);
        RuntimeDetails.InnerClassValidation(x, x).Assert().Is(true);
    }
}
