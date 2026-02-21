using System.Reflection;

namespace CreateAndFake.xUnit.v3.Tests;

public static class XUnitTests
{
    [Fact]
    internal static Task XUnit_v3_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(
            ToolSet.DefaultSet,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void XUnit_v3_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task XUnit_v3_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }
}
