using System.Reflection;

namespace CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class MSTestTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task MSTest_v3_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(
            ToolSet.DefaultSet,
            TestContext.CancellationToken
        );
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
    public Task MSTest_v3_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.CancellationToken
        );
    }
}
