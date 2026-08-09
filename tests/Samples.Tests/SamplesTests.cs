using System.Reflection;

namespace Werecodent.CreateAndFake.Samples.Tests;

public static class SamplesTests
{
    private static readonly Assembly _SampleAssembly = Assembly.GetAssembly(
        typeof(SampleGenerator)
    );

    [Fact]
    internal static void Samples_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            _SampleAssembly,
            Assembly.GetExecutingAssembly(),
            opt =>
                opt with
                {
                    TestClassCoverageExceptions =
                    [
                        .. _SampleAssembly
                            .GetTypes()
                            .Where(t => Attribute.IsDefined(t, typeof(InvalidSampleAttribute)))
                            .Select(t => t.Name),
                    ],
                }
        );
    }

    [Fact]
    internal static void Samples_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(FactAttribute), typeof(TheoryAttribute)],
            _SampleAssembly,
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task Samples_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Samples_ValidSamplesWork()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            SampleGenerator.AllValidDataSamples,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Samples_VerifyAllToStrings()
    {
        Tools.Tester.VerifyAllToStrings(_SampleAssembly);
    }
}
