using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

internal static class StaticMutationSample
{
    public static void Mutate(DataHolderSample data)
    {
        data.NestedValue = null;
    }
}
