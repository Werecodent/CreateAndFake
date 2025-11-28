using System.Reflection;

namespace CreateAndFake.NUnit.v3.Tests;

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

    [Test]
    public static Task NUnit_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParameters(Assembly.GetExecutingAssembly());
    }
}
