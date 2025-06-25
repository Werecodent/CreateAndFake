using System.Reflection;

namespace CreateAndFake.NUnit.Tests;

[TestFixture]
public static class NUnitTests
{
    [Test]
    public static void NUnit_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(RandomDataAttribute)),
            Assembly.GetExecutingAssembly()
        );
    }
}
