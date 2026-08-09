using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Engine;

public static class CreateHintResultTests
{
    [Fact]
    internal static Task CreateHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CreateHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CreateHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CreateHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CreateHintResult.None.HasData.Assert().Is(false);
    }
}
