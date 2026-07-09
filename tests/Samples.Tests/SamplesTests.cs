using System.Reflection;
using CreateAndFake.AsserterTool;

namespace CreateAndFake.Samples.Tests;

public static class SamplesTests
{
    [Fact]
    internal static void Samples_Tests_TestClassCoverage()
    {
        Assembly
            .GetAssembly(typeof(SampleGenerator))
            .Assert(x => Tools.Tester.ProvidesTestClassCoverage(x, Assembly.GetExecutingAssembly()))
            .Throws<AssertException>();
    }
}
