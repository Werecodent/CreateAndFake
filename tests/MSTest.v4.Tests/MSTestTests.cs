using System.Reflection;

namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class MSTestTests
{
    [TestMethod]
    public void MSTest_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }

    [TestMethod]
    public Task MSTest_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParameters(Assembly.GetExecutingAssembly());
    }
}
