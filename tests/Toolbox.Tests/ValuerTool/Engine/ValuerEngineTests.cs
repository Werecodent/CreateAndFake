using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerEngineTests
{
    [Fact]
    internal static Task ValuerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ValuerEngine>();
    }

    [Fact]
    internal static Task ValuerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ValuerEngine>();
    }
}
