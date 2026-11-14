using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public sealed class BaseInjectAttributeTests : AutoTester<BaseInjectAttribute>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };
}
