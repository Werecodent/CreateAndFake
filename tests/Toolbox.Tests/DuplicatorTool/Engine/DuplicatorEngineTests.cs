using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorEngineTests
{
    [Fact]
    internal static Task DuplicatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DuplicatorEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                IgnorableExceptions =
                [
                    typeof(ArgumentException),
                    typeof(TargetParameterCountException),
                    typeof(NotSupportedException),
                ],
            }
        );
    }

    [Fact]
    internal static Task DuplicatorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DuplicatorEngine>(opt =>
            opt with
            {
                InjectionValues = [Tools.Duplicator.Options, Duplicator.DefaultHints],
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                IgnorableExceptions =
                [
                    typeof(ArgumentException),
                    typeof(TargetParameterCountException),
                    typeof(NotSupportedException),
                ],
            }
        );
    }
}
