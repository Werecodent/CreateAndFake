using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class DisposerTests() : AutoTester(typeof(Disposer))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { DisableParameterMutationTests = true };

    [Theory, RandomData]
    internal static void Cleanup_DisposesAllItems(
        [Stub] IDisposable item1,
        [Stub] IDisposable item2
    )
    {
        item1.SetupReturn(m => m.Dispose(), Behavior.None(Times.Once));
        item2.SetupReturn(m => m.Dispose(), Behavior.None(Times.Once));

        Disposer.Cleanup(item1, new object(), item2, "");

        item1.Assert().Called().Also(item2).Called();
    }

    [Theory, RandomData]
    internal static async Task CleanupAsync_DisposesAllItems(
        [Stub] IDisposable item1,
        [Stub] IAsyncDisposable item2
    )
    {
        item1.SetupReturn(m => m.Dispose(), Behavior.None(Times.Once));
        item2
            .Tools()
            .ToFake()
            .Setup(m => m.DisposeAsync(), Behavior.Returns<ValueTask>(default, Times.Once));

        await Disposer.CleanupAsync(item1, new object(), item2, "");

        item1.Assert().Called().Also(item2).Called();
    }

    [Fact]
    internal static async Task CleanupAsync_PrioritizesAsync()
    {
        Fake<IAsyncDisposable> item = Tools.Faker.Mock<IAsyncDisposable>(typeof(IDisposable));
        item.Setup(m => m.DisposeAsync(), Behavior.Returns<ValueTask>(default, Times.Once));

        await Disposer.CleanupAsync(item.Dummy);

        item.Verify();
    }
}
