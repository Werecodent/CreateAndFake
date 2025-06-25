using System.Reflection;

namespace CreateAndFake.xUnit.v2.Tests;

public static class XUnit_v2Tests
{
    [Fact]
    internal static void XUnit_v2_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }
}
