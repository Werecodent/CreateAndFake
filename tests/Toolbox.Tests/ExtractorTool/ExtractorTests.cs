using Newtonsoft.Json;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

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
        foreach (
            Type type in CollectionCreateHint.PotentialCollections.Where(t =>
                !ArgumentGuard.IsAsynchronous(t)
            )
        )
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

#pragma warning disable MA0042, VSTHRD103 // Behavior specifically being tested.

    [Theory, RandomData]
    internal static async Task ExtractAsync_MatchesExtract(DataHolderSample sample)
    {
        ISet<object> asyncContent = (
            await AsyncSeriesHelper.ToListAsync(
                (
                    await Tools.Extractor.ExtractAsync(
                        sample,
                        TestContext.Current.CancellationToken
                    )
                ).AllContentAsync(TestContext.Current.CancellationToken),
                Tools.Valuer.Options.IterationLimit,
                TestContext.Current.CancellationToken
            )
        ).ToHashSet();

        ISet<object> syncContent = Tools.Extractor.Extract(sample).AllContent().ToHashSet();

        asyncContent
            .Assert()
            .Is(syncContent, JsonConvert.SerializeObject(sample, Formatting.Indented));
    }

#pragma warning restore
}
