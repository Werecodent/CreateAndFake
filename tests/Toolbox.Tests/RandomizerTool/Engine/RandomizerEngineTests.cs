using System.Collections.Frozen;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerEngineTests
{
    [Fact]
    internal static Task RandomizerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            new RandomizerEngine(),
            TestContext.Current.CancellationToken,
            opt => opt with { MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints", "Inject"]) }
        );
    }

    [Fact]
    internal static Task RandomizerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            new RandomizerEngine(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Randomizer.Options],
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["Create", "SelectHints", "Inject"]),
                }
        );
    }
}
