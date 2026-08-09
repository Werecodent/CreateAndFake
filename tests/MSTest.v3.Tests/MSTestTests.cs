using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class MSTestTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task MSTest_v3_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public void MSTest_v3_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [TestMethod]
    public void MSTest_v3_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(TestMethodAttribute)],
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly(),
            opt => opt with { TestMethodNameAllowedTargets = ["Issue118"] }
        );
    }

    [TestMethod]
    public Task MSTest_v3_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.CancellationToken
        );
    }
}
