using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class FactoryCreateHandler_T_Tests
{
    [Fact]
    internal static Task FactoryCreateHandler_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(FactoryCreateHandler<object>),
            TestContext.Current.CancellationToken
        );
    }
}
