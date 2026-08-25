using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncListTests
{
    [Fact]
    internal static void Debug_AsyncList_ToString()
    {
        typeof(AsyncList<>).Tools().CreateRandomInstance().ToString().Assert().Debug();
    }

    [Fact]
    internal static Task AsyncList_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncList_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncList<>),
            TestContext.Current.CancellationToken
        );
    }
}
