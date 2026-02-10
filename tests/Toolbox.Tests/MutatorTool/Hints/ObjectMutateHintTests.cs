using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class ObjectMutateHintTests
{
    [Fact]
    internal static Task ObjectMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ObjectMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
