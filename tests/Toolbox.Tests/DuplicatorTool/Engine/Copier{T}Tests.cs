using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class Copier_T_Tests
{
    [Fact]
    internal static Task Copier_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(Copier<object>),
            TestContext.Current.CancellationToken
        );
    }
}
