using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorChainerTests
{
    [Fact]
    internal static Task MutatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<MutatorChainer>(
            TestContext.Current.CancellationToken
        );
    }
}
