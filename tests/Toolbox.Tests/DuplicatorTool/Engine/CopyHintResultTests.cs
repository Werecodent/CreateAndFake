using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class CopyHintResultTests
{
    [Fact]
    internal static Task CopyHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CopyHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CopyHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CopyHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CopyHintResult.None.HasData.Assert().Is(false);
    }
}
