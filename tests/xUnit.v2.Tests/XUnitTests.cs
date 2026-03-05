using System.Reflection;

namespace CreateAndFake.xUnit.v2.Tests;

public static class XUnitTests
{
    [Fact]
    internal static Task XUnit_v2_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(CancellationToken.None);
    }

    [Fact]
    internal static void XUnit_v2_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task XUnit_v2_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            CancellationToken.None
        );
    }
}
