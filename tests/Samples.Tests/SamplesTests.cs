using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.Tests;

public static class SamplesTests
{
    [Fact]
    internal static void Samples_Tests_TestClassCoverage()
    {
        Assembly
            .GetAssembly(typeof(IReadableHolder<>))
            .Assert(assembly =>
                Tools.Tester.ProvidesTestClassCoverage(assembly, Assembly.GetExecutingAssembly())
            )
            .Throws<AssertException>();
    }
}
