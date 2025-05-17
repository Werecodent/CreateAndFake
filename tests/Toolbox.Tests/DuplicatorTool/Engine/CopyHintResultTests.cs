using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class CopyHintResultTests
{
    [Fact]
    internal static Task CopyHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<CopyHintResult>();
    }

    [Fact]
    internal static Task CopyHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<CopyHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CopyHintResult.None.HasData.Assert().Is(false);
    }
}
