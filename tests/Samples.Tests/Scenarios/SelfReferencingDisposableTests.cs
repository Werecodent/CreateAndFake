using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public sealed class SelfReferencingDisposableTests : AutoTester<SelfReferencingDisposable>
{
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
