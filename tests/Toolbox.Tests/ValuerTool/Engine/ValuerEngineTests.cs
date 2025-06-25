using System.Collections.Frozen;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerEngineTests
{
    [Fact]
    internal static Task ValuerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ValuerEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }

    [Fact]
    internal static Task ValuerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ValuerEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }
}
