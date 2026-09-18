using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class HashCodeHintResultTests
{
    [Fact]
    internal static Task HashCodeHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<HashCodeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task HashCodeHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<HashCodeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintResult.None.HasData.Assert().Is(false);
    }
}
