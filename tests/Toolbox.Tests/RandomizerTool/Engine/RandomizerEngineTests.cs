using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerEngineTests
{
    [Fact]
    internal static Task RandomizerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IRandomizerEngine>();
    }

    [Fact]
    internal static Task RandomizerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IRandomizerEngine>();
    }
}
