using System.Reflection;

namespace CreateAndFake.xUnit.v3.Tests;

public static class XUnitTests
{
    [Fact]
    internal static void XUnit_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task XUnit_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParameters(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }
}
