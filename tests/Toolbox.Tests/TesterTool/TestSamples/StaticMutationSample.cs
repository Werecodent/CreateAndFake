using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.TesterTool.TestSamples;

internal static class StaticMutationSample
{
    public static void Mutate(DataHolderSample data)
    {
        data.NestedValue = null;
    }
}
