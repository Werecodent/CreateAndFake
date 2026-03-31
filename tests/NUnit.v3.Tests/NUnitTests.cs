using System.Reflection;

namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class NUnitTests
{
    [Test]
    public static Task NUnit_v3_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static void NUnit_v3_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Test]
    public static void NUnit_v3_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(TestAttribute)],
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly(),
            opt => opt with { TestMethodNameAllowedTargets = ["Issue118"] }
        );
    }

    [Test]
    public static Task NUnit_v3_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.CurrentContext.CancellationToken
        );
    }
}
