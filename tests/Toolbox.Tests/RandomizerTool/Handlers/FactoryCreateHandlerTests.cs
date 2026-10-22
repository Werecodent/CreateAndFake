using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public static class FactoryCreateHandlerTests
{
    [Fact]
    internal static Task FactoryCreateHandler_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FactoryCreateHandler>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FactoryCreateHandler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FactoryCreateHandler>(
            TestContext.Current.CancellationToken
        );
    }
}
