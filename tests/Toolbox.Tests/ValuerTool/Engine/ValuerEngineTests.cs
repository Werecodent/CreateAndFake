using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerEngineTests
{
    [Fact]
    internal static Task ValuerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions = [typeof(UnsupportedException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static Task ValuerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Valuer.Options],
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions =
                    [
                        typeof(UnsupportedException),
                        typeof(TargetException),
                        typeof(ToolException),
                    ],
                }
        );
    }
}
