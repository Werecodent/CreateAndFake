using CreateAndFake.RandomizerTool;

namespace CreateAndFake.Tests.RandomizerTool;

public static class CreateHintResultTests
{
    [Fact]
    internal static void CreateHintResult_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<CreateHintResult>();
    }

    [Fact]
    internal static void CreateHintResult_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<CreateHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        CreateHintResult.None.HasData.Assert().Is(false);
    }
}
