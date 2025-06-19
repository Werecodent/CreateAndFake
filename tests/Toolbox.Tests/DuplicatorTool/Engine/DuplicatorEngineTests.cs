using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorEngineTests
{
    [Fact]
    internal static Task DuplicatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IDuplicatorEngine>();
    }

    [Fact]
    internal static Task DuplicatorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IDuplicatorEngine>();
    }
}
