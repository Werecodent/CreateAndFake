using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public static class SetupHintResultTests
{
    [Fact]
    internal static Task SetupHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SetupHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SetupHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SetupHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        SetupHintResult.None.HasData.Assert().Is(false);
    }
}
