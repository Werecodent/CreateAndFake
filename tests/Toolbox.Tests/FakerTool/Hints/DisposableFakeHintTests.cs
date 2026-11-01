using Werecodent.CreateAndFake.FakerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Hints;

public sealed class DisposableFakeHintTests : AutoTester<DisposableFakeHint>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(ArgumentException), typeof(NotSupportedException)],
            };
}
