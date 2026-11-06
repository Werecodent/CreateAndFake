using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public sealed class ObjectToolsTests : AutoTester<ObjectTools<IFaked>>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };

    public override Task Tester_ValueEqualityOverrideCheck()
    {
        return Task.CompletedTask;
    }
}
