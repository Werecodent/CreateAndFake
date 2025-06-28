using System.Collections.Frozen;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorEngineTests
{
    [Fact]
    internal static Task ExtractorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ExtractorEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }

    [Fact]
    internal static Task ExtractorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ExtractorEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
            }
        );
    }
}
