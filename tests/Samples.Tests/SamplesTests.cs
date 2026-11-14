using System.Reflection;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Samples.Tests;

public sealed class SamplesTests : AssemblyTester
{
    private static readonly Assembly _SampleAssembly = Assembly.GetAssembly(
        typeof(SampleGenerator)
    );

    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                ClassCoverageExclusions =
                [
                    .. _SampleAssembly
                        .GetTypes()
                        .Where(t => Attribute.IsDefined(t, typeof(InvalidSampleAttribute)))
                        .Select(t => t.Name),
                ],
            };

    protected override Assembly CodeAssembly { get; } = _SampleAssembly;

    [Fact]
    internal static Task Samples_ValidSamplesWork()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            SampleGenerator.AllValidDataSamples,
            TestContext.Current.CancellationToken
        );
    }
}
