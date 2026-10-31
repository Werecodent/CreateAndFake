namespace Werecodent.CreateAndFake.Samples.Tests;

public sealed class SampleGeneratorTests() : AutoTester(typeof(SampleGenerator))
{
    [Fact]
    internal static void Debug_SampleGenerator_AllValidSamples()
    {
        SampleGenerator.AllValidDataSamples.Assert().Debug();
    }
}
