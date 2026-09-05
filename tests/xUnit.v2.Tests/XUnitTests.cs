using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

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
    internal static void XUnit_v2_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(FactAttribute), typeof(TheoryAttribute)],
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly(),
            opt => opt with { TestMethodNameAllowedTargets = ["Issue118"] }
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
