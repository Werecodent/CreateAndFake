using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutateHintResultTests
{
    [Fact]
    internal static Task MutateHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MutateHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task MutateHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<MutateHintResult>(
            TestContext.Current.CancellationToken
        );
    }
}
