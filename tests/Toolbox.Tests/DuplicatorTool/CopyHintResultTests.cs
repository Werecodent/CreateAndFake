using CreateAndFake.DuplicatorTool;

namespace CreateAndFake.Tests.DuplicatorTool;

public static class CopyHintResultTests
{
    [Fact]
    internal static void CopyHintResult_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<CopyHintResult>();
    }

    [Fact]
    internal static void CopyHintResult_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<CopyHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CopyHintResult.None.HasData.Assert().Is(false);
    }
}
