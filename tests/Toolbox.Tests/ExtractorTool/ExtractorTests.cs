using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.RandomizerTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool;

public static class ExtractorTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task Extractor_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Extractor>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Extractor_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Extractor>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void Extract_CollectionsWork()
    {
        foreach (Type type in CollectionCreateHint.PotentialCollections)
        {
            Tools
                .Extractor.Extract(Tools.Randomizer.Create(type))
                .AllContent()
                .Assert()
                .IsNotEmpty();
        }
    }

    [Fact]
    internal static void Extract_LegacyCollectionsWork()
    {
        foreach (Type type in LegacyCollectionCreateHint.PotentialCollections)
        {
            Tools
                .Extractor.Extract(Tools.Randomizer.Create(type))
                .AllContent()
                .Assert()
                .IsNotEmpty();
        }
    }
}
