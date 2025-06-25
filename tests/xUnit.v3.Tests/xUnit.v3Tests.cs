using System.Reflection;

namespace CreateAndFake.xUnit.v3.Tests;

public static class XUnit_v3Tests
{
    [Fact]
    internal static void XUnit_v3_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }
}
