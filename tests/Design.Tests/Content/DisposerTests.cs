using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Design.Tests.Content;

public static class DisposerTests
{
    [Fact]
    internal static Task Disposer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(Disposer),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Disposer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(Disposer),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Cleanup_DisposesAllItems(Fake<IDisposable> item1, Fake<IDisposable> item2)
    {
        item1.Setup(m => m.Dispose(), Behavior.None(Times.Once));
        item2.Setup(m => m.Dispose(), Behavior.None(Times.Once));
        Disposer.Cleanup(item1.Dummy, new object(), item2.Dummy, "");
        item1.VerifyAll();
        item2.VerifyAll();
    }

    [Theory, RandomData]
    internal static async Task CleanupAsync_DisposesAllItems(
        Fake<IDisposable> item1,
        Fake<IAsyncDisposable> item2
    )
    {
        item1.Setup(m => m.Dispose(), Behavior.None(Times.Once));
        item2.Setup(m => m.DisposeAsync(), Behavior.Returns<ValueTask>(default, Times.Once));
        await Disposer.CleanupAsync(item1.Dummy, new object(), item2.Dummy, "");
        item1.VerifyAll();
        item2.VerifyAll();
    }

    [Fact]
    internal static async Task CleanupAsync_PrioritizesAsync()
    {
        Fake<IAsyncDisposable> item = Tools.Faker.Mock<IAsyncDisposable>(typeof(IDisposable));
        item.Setup(m => m.DisposeAsync(), Behavior.Returns<ValueTask>(default, Times.Once));

        await Disposer.CleanupAsync(item.Dummy);

        item.VerifyAll();
    }
}
