using System.Collections.Frozen;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public sealed class DuplicatorEngineTests : AutoTester<DuplicatorEngine>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                InjectionValues = [Tools.Duplicator.Options],
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                IgnorableExceptions =
                [
                    typeof(ArgumentException),
                    typeof(TargetParameterCountException),
                    typeof(UnsupportedException),
                    typeof(ToolException),
                ],
            };
}
