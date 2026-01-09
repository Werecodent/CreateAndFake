using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class CreateHintResultTests
{
    [Fact]
    internal static Task CreateHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<CreateHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CreateHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<CreateHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CreateHintResult.None.HasData.Assert().Is(false);
    }
}
