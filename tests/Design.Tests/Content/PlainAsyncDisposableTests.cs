using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class PlainAsyncDisposableTests
{
    [Fact]
    internal static Task AsyncDisposable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<PlainAsyncDisposable>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncDisposable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<PlainAsyncDisposable>(
            TestContext.Current.CancellationToken
        );
    }
}
