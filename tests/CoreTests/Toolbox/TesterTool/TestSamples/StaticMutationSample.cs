using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

internal static class StaticMutationSample
{
    internal static void Mutate(DataHolderSample data)
    {
        data.NestedValue = null;
    }
}
