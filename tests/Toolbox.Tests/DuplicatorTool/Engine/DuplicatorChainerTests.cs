using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public sealed class DuplicatorChainerTests : AutoTester<DuplicatorChainer>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions =
                [
                    typeof(UnsupportedException),
                    typeof(TargetParameterCountException),
                    typeof(ArgumentException),
                    typeof(ToolException),
                ],
            };
}
