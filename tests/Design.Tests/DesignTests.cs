using System.Reflection;

namespace Werecodent.CreateAndFake.Design.Tests;

public static class DesignTests
{
    private static readonly Assembly _DesignAssembly = Assembly.GetAssembly(typeof(ArgumentGuard));

    [Fact]
    internal static void Design_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(_DesignAssembly, Assembly.GetExecutingAssembly());
    }

    [Fact]
    internal static void Design_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(FactAttribute), typeof(TheoryAttribute)],
            _DesignAssembly,
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task Design_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Design_VerifyAllToStrings()
    {
        Tools.Tester.VerifyAllToStrings(_DesignAssembly);
    }
}
