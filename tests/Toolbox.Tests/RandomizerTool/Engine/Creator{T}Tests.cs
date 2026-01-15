using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class Creator_T_Tests
{
    [Fact]
    internal static Task Creator_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(Creator<object>),
            TestContext.Current.CancellationToken
        );
    }
}
