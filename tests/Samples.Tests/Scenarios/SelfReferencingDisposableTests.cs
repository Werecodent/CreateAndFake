using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class SelfReferencingDisposableTests
{
    [Fact]
    public static Task SelfReferencingDisposable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SelfReferencingDisposable>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task SelfReferencingDisposable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SelfReferencingDisposable>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void GrabData_ThrowsWhenDisposed(SelfReferencingDisposable sample)
    {
        sample.Dispose();
        sample.Assert(x => x.GrabData()).Throws<ObjectDisposedException>();
    }

    [Theory, RandomData]
    internal static async Task GetSelf_ThrowsWhenDisposed(SelfReferencingDisposable sample)
    {
        await sample.DisposeAsync();
        sample.Assert(x => x.GetSelf()).Throws<ObjectDisposedException>();
    }
}
