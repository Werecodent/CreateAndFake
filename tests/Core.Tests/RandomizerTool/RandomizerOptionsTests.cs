global using RandomizerMod = System.Func<
    CreateAndFake.RandomizerTool.RandomizerOptions,
    CreateAndFake.RandomizerTool.RandomizerOptions>;

using CreateAndFake.RandomizerTool;

namespace CreateAndFake.Tests.RandomizerTool;

public static class RandomizerOptionsTests
{
    [Fact]
    internal static void RandomizerOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RandomizerOptions>();
    }

    [Fact]
    internal static void RandomizerOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RandomizerOptions>();
    }

    [Fact]
    internal static void RandomizerOptions_ModFormRandomizable()
    {
        typeof(RandomizerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}