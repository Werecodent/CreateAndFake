using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerEngineTests
{
    [Fact]
    internal static Task ValuerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ValuerEngine>(opt =>
            opt with
            {
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                IgnorableExceptions = [typeof(NotSupportedException)],
            }
        );
    }

    [Fact]
    internal static Task ValuerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ValuerEngine>(opt =>
            opt with
            {
                InjectionValues = [Tools.Valuer.Options, Valuer.DefaultHints],
                MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                IgnorableExceptions =
                [
                    typeof(NotSupportedException),
                    typeof(TargetException),
                    typeof(InsufficientExecutionStackException),
                ],
            }
        );
    }
}
