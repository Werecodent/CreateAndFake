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

    [Theory, RandomData]
    internal static async Task GrabData_ThrowsWhenDisposed(PlainAsyncDisposable sample)
    {
        await sample.DisposeAsync();
        sample.Assert(x => x.GrabData()).Throws<ObjectDisposedException>();
    }
}
