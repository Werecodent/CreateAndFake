using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class FactoryCopyHandler_T_Tests
{
    [Fact]
    internal static Task FactoryCopyHandler_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(FactoryCopyHandler<object>),
            TestContext.Current.CancellationToken
        );
    }
}
