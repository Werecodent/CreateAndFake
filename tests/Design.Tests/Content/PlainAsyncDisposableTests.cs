using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class PlainAsyncDisposableTests : AutoTester<PlainAsyncDisposable>
{
    [Theory, RandomData]
    internal static async Task GrabData_ThrowsWhenDisposed(PlainAsyncDisposable sample)
    {
        await sample.DisposeAsync();
        sample.Assert(x => x.GrabData()).Throws<ObjectDisposedException>();
    }
}
