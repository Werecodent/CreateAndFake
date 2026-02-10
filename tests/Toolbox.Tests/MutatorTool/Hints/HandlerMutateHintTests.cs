using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class HandlerMutateHintTests
{
    [Fact]
    internal static Task HandlerMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<HandlerMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
