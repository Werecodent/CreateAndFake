using System.Collections.Frozen;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerEngineTests
{
    [Fact]
    internal static Task RandomizerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RandomizerEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }

    [Fact]
    internal static Task RandomizerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RandomizerEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }
}
