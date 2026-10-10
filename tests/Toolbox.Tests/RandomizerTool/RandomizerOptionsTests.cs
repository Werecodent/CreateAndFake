global using RandomizerMod = System.Func<
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions,
    Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions
>;
using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool;

public static class RandomizerOptionsTests
{
    [Fact]
    internal static Task RandomizerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RandomizerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RandomizerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RandomizerOptions>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentNullException)] }
        );
    }

    [Fact]
    internal static void RandomizerOptions_ModFormRandomizable()
    {
        typeof(RandomizerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }

    [Fact]
    internal static void NextStringSize_PreventsOverflow()
    {
        (
            Tools.Randomizer.Options with
            {
                StringMinSize = int.MaxValue,
                StringMaxSize = int.MaxValue,
            }
        )
            .NextStringSize()
            .Assert()
            .Is(int.MaxValue);
    }
}
