using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class DisposerTests
{
    [Fact]
    internal static Task Disposer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(Disposer));
    }

    [Fact]
    internal static Task Disposer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(Disposer));
    }
}
