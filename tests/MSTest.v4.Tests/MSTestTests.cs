using System.Reflection;

namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class MSTestTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task MSTest_v4_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public void MSTest_v4_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [TestMethod]
    public Task MSTest_v4_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.CancellationToken
        );
    }
}
