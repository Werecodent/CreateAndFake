using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class Creator_T_Tests
{
    [Fact]
    internal static Task CreatorT_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(Creator<object>),
            TestContext.Current.CancellationToken
        );
    }
}
