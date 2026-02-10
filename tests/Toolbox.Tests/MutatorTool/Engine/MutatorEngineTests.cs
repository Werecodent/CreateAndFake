using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorEngineTests
{
    [Fact]
    internal static Task MutatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<MutatorEngine>(
            TestContext.Current.CancellationToken
        );
    }
}
