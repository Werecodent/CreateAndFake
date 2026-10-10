using System.Collections.Frozen;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerEngineTests
{
    [Fact]
    internal static Task RandomizerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomizerEngine(),
            TestContext.Current.CancellationToken,
            opt => opt with { MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints", "Inject"]) }
        );
    }

    [Fact]
    internal static Task RandomizerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
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
