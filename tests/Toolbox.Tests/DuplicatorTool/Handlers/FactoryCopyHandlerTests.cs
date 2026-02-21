using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class FactoryCopyHandlerTests
{
    [Fact]
    internal static Task FactoryCopyHandler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FactoryCopyHandler>(
            TestContext.Current.CancellationToken
        );
    }
}
