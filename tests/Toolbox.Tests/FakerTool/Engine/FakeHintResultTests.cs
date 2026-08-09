using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public static class FakeHintResultTests
{
    [Fact]
    internal static Task FakeHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FakeHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        FakeHintResult.None.HasData.Assert().Is(false);
    }
}
