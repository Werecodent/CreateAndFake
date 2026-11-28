using Werecodent.CreateAndFake.FakerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Hints;

public sealed class ObjectFakeHintTests : AutoTester<ObjectFakeHint>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(ArgumentException), typeof(NotSupportedException)],
            };
}
