using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class ValuerChainerTests : AutoTester<ValuerChainer>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions =
                [
                    typeof(UnsupportedException),
                    typeof(ToolException),
                    typeof(EngineException),
                ],
            };
}
