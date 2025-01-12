global using RandomizerMod = System.Func<
    CreateAndFake.Toolbox.RandomizerTool.RandomizerOptions,
    CreateAndFake.Toolbox.RandomizerTool.RandomizerOptions>;

using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFakeTests.Toolbox.RandomizerTool;

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