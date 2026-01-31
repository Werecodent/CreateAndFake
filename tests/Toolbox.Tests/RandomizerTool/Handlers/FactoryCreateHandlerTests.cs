using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class FactoryCreateHandlerTests
{
    [Fact]
    internal static Task FactoryCreateHandler_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FactoryCreateHandler>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FactoryCreateHandler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FactoryCreateHandler>(
            TestContext.Current.CancellationToken
        );
    }
}
