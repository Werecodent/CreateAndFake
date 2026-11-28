using Werecodent.CreateAndFake.FakerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Hints;

public sealed class AsyncDisposableFakeHintTests : AutoTester<AsyncDisposableFakeHint>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(ArgumentException), typeof(NotSupportedException)],
            };
}
