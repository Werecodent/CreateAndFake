using System.Collections.Frozen;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorEngineTests
{
    [Fact]
    internal static Task DuplicatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DuplicatorEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }

    [Fact]
    internal static Task DuplicatorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DuplicatorEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }
}
