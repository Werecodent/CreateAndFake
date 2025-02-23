using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class DisposerTests
{
    [Fact]
    internal static void Disposer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(Disposer));
    }

    [Fact]
    internal static void Disposer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(Disposer));
    }
}
