using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool;

public static class ExtractorTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task Extractor_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Extractor>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task Extractor_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Extractor>(
            TestContext.Current.CancellationToken,
            _Config
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
