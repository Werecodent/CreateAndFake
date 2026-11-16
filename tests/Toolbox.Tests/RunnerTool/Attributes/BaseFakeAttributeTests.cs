using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public sealed class BaseFakeAttributeTests : AutoTester<BaseFakeAttribute>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableToolSetSupportTests = true,
                IgnorableExceptions = [typeof(ToolException), typeof(ArgumentException)],
            };
}
