using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class FactoryCreateHandler_T_Tests
{
    [Fact]
    internal static Task FactoryCreateHandler_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(FactoryCreateHandler<>),
            TestContext.Current.CancellationToken
        );
    }
}
