using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.TesterTool.TestSamples;

internal static class StaticMutationSample
{
    internal static void Mutate(DataHolderSample data)
    {
        data.NestedValue = null;
    }
}
