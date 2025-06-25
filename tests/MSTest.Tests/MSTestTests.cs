using System.Reflection;

namespace CreateAndFake.MSTest.Tests;

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
}
