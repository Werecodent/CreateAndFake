using CreateAndFake.Toolbox.ExtractorTool;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;

namespace CreateAndFakeTests.Toolbox.ExtractorTool;

public static class ExtractorTests
{
    [Fact]
    internal static void Extractor_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Extractor>();
    }

    [Fact]
    internal static void Extractor_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Extractor>();
    }

    [Fact]
    internal static void Extract_CollectionsWork()
    {
        foreach (Type type in CollectionCreateHint.PotentialCollections)
        {
            Tools.Extractor.Extract(Tools.Randomizer.Create(type)).AllContent().Assert().IsNotEmpty();
        }
    }

    [Fact]
    internal static void Extract_LegacyCollectionsWork()
    {
        foreach (Type type in LegacyCollectionCreateHint.PotentialCollections)
        {
            Tools.Extractor.Extract(Tools.Randomizer.Create(type)).AllContent().Assert().IsNotEmpty();
        }
    }
}
